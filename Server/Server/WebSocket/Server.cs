using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using WebSocket.Exceptions;
using WebSocket.Model;
using WebSocket.Protocol;


namespace WebSocket
{
    /// <summary>
    /// Classe qui se charge de communiquer avec des clients 
    /// </summary>
    public class Server
    {
        private IWebProtocol webSocket;
        private bool isRunning;
        private bool started;
        private string gameType;
        private static ConcurrentDictionary<int, Game> customGames = new ConcurrentDictionary<int, Game>();
        private static ConcurrentDictionary<int, Game> matchmakingGames = new ConcurrentDictionary<int, Game>();
        private static ConcurrentDictionary<int, Lobby> lobbies = new ConcurrentDictionary<int, Lobby>();
        private static readonly Queue<Client> waitingPlayers = new Queue<Client>();

        private Interpreter interpreter;
        private GameManager gameManager;

        /// <summary>
        /// Dictionnaire qui contient les parties personnalisées en cours
        /// </summary>
        public static ConcurrentDictionary<int, Game> CustomGames { get => customGames; set => customGames = value; }

        /// <summary>
        /// Dictionnaire qui contient les parties de matchmaking en cours
        /// </summary>
        public static ConcurrentDictionary<int, Game> MatchmakingGames { get => matchmakingGames; set => matchmakingGames = value; }
        public static ConcurrentDictionary<int, Lobby> Lobbies { get => lobbies; set => lobbies = value; }

        public static Queue<Client> WaitingPlayers => waitingPlayers;


        /// <summary>
        /// Constructeur de la classe Server
        /// </summary>
        public Server()
        {
            this.webSocket = new Protocol.WebSocket("10.211.55.3", 7000); //10.211.55.3
            this.gameManager = new GameManager();
        }


        /// <summary>
        /// Démarre l'écoute du serveur 
        /// </summary>
        public void Start()
        {
            this.webSocket.Start();
            this.isRunning = true;
            Console.WriteLine("Server Started");

            while (isRunning)
            {
                try
                {
                    Thread thread = new Thread(() =>
                    {
                        TcpClient tcp = this.webSocket.AcceptClient();
                        Client client = new Client(tcp);
                        this.interpreter = new Interpreter();
                        bool endOfCommunication = false;


                        while (!endOfCommunication)
                        {
                            byte[] bytes = client.ReceiveMessage();
                            string message = Encoding.UTF8.GetString(bytes);
                            string response = "";

                            if (this.MessageIsHandshakeRequest(message)) // test si le message reçu est une demande de handshake
                            {
                                this.ProceedHandshake(message, client, ref response);
                            }
                            else // Le message est un message chiffré
                            {
                                try
                                {
                                    this.TreatMessage(bytes, client, ref message, ref response);
                                }
                                catch (DisconnectionException ex) // Le message reçu est un message de déconnexion
                                {
                                    this.DisconnectClient(client, ex, ref endOfCommunication);
                                }
                            }
                            if (!endOfCommunication)
                            {
                                Console.WriteLine($"Received : {message}");
                                Console.WriteLine($"Sent : {response}");
                            }
                        }

                    });
                    thread.Start();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Vérifie si le message reçu est une demande de handshake
        /// </summary>
        public bool MessageIsHandshakeRequest(string message)
        {
            return Regex.IsMatch(message, "^GET");
        }

        /// <summary>
        /// Procède à l'établissement de la connexion avec le client
        /// </summary>
        private void ProceedHandshake(string message, Client client, ref string response)
        {
            byte[] handshake = this.webSocket.BuildHandShake(message);
            response = Encoding.UTF8.GetString(handshake);
            client.SendMessage(handshake);
        }

        /// <summary>
        /// Déconnecte un joueur de sa partie
        /// </summary>
        private void DisconnectClient(Client client, DisconnectionException ex, ref bool endOfCommunication)
        {
            foreach (var game in Server.CustomGames)
            {
                if (game.Value.Player1 == client)
                {
                    game.Value.Player1 = null;
                }
                else if (game.Value.Player2 == client)
                {
                    game.Value.Player2 = null;
                }
            }
            byte[] deconnectionBytes = this.webSocket.BuildDeconnection(ex.Code);
            client.SendMessage(deconnectionBytes);
            Console.WriteLine(ex.Message + "\n");
            endOfCommunication = true; // Fin de la communication
            this.started = false;
        }


        /// <summary>
        /// Traite le message reçu par le client
        /// </summary>
        private void TreatMessage(byte[] bytes, Client client, ref string message, ref string response)
        {
            byte[] decryptedMessage = this.webSocket.DecryptMessage(bytes);
            message = Encoding.UTF8.GetString(decryptedMessage);        
            if (message.Contains("custom"))
            {
                this.gameType = "custom";
            }
            else if (message.Contains("matchmaking"))
            {
                this.gameType = "matchmaking";
            }
            response = this.interpreter.Interpret(message, client, this.gameType); // Interprétation du message reçu
            string[] data = response.Split("_");
            string responseType = data[0]; // Récupération du type de réponse (Send ou Broadcast)
            string responseData = data[1]; // Récupération des données à envoyer


            string stringId = responseData.Split("-")[0];
            int idGame = Convert.ToInt32(stringId); // Id de la partie concernée
            byte[] responseBytes = this.webSocket.BuildMessage(responseData);

            if (!response.Contains("Create") && (!response.Contains("Timeout")) && (!response.Contains("Cancelled")) && (!response.Contains("Retry")))
            {
                Game game = this.gameType == "custom" ? customGames[idGame] : matchmakingGames[idGame];
                if (responseType == "Broadcast")
                {
                    this.BroadcastMessage(game, responseBytes);
                }
                if (game.IsFull && !game.Started)
                {
                    this.StartGame(game);
                }

            }
            else if(responseType == "Broadcast")
            {
                this.BroadcastCancelMessage(Server.Lobbies[idGame], responseBytes);
            }

            if (responseType == "Send")
            {
                this.SendMessage(client, responseBytes);
            }
            response = responseData;
        }

        private void SendMessage(Client client, byte[] bytes)
        {
            if (client != null)
            {
                client.SendMessage(bytes);
            }
        }

        private void BroadcastMessage(Game game, byte[] bytes)
        {
            this.SendMessage(game.Player1, bytes);
            this.SendMessage(game.Player2, bytes);

            if (game.TestWin()) // Appel direct de la méthode TestWin() de l'objet Game
            {
                this.handleGameEnd(game);
            }
        }

        private void BroadcastCancelMessage(Lobby lobby, byte[] bytes)
        {
            this.SendMessage(lobby.Player1, bytes);
            this.SendMessage(lobby.Player2, bytes);
            Server.Lobbies.TryRemove(lobby.Id, out _);

        }


        /// <summary>
        /// Démarre une partie
        /// </summary>
        private void StartGame(Game game)
        {
            game.Player1.User = this.gameManager.GetUserByToken(game.Player1.User.Token);
            game.Player2.User = this.gameManager.GetUserByToken(game.Player2.User.Token);
            byte[] startP1 = this.webSocket.BuildMessage($"{game.Id}-Start-{game.Player2.User.Name}"); // Envoi du nom du joueur à son adversaire
            byte[] startP2 = this.webSocket.BuildMessage($"{game.Id}-Start-{game.Player1.User.Name}"); // Envoi du nom du joueur à son adversaire
            this.SendMessage(game.Player1, startP1);
            this.SendMessage(game.Player2, startP2);
            game.Start();
        }

        /// <summary>
        /// Gère la fin de partie en récupérant le score final des deux joueurs et en renvoyant le gagnant aux deux joueurs 
        /// </summary>
        private void handleGameEnd(Game game)
        {
            (int,int) scores = game.GetScore();
            int scorePlayer1 = scores.Item1;
            int scorePlayer2 = scores.Item2;
            bool player1won = false;
            bool player2won = false;

            //Maj de l'elo en fonction du gagnant
            //et renvoi à chaque joueur un bouléen indiquant si il a gagné ou non
            if (scorePlayer1 >= scorePlayer2)
            {
                this.gameManager.UpdateEloWinnerLooser(game.Player1.User, game.Player2.User);
                player1won= true;
                
            }
            else
            {
                this.gameManager.UpdateEloWinnerLooser(game.Player2.User, game.Player1.User);
                player2won= true;
            }

            byte[] endOfGameMessagePlayer1 = this.webSocket.BuildMessage($"{game.Id}-EndOfGame-{scorePlayer1}-{scorePlayer2}-{player1won}");
            byte[] endOfGameMessagePlayer2 = this.webSocket.BuildMessage($"{game.Id}-EndOfGame-{scorePlayer2}-{scorePlayer1}-{player2won}");
            this.SendMessage(game.Player1, endOfGameMessagePlayer1);
            this.SendMessage(game.Player2, endOfGameMessagePlayer2);
        }

        /// <summary>
        /// Test si un joueur a gagné
        /// </summary>
        private bool TestWin(Game game)
        {
            return game.TestWin();
        }
 
    }

}
