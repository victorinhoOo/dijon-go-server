using MySqlX.XDevAPI;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using WebSocket.Exceptions;
using WebSocket.Model;
using WebSocket.Model.DAO.Redis;
using WebSocket.Model.Managers;
using WebSocket.Protocol;
using WebSocket.Strategy.Enumerations;


namespace WebSocket
{
    /// <summary>
    /// Classe qui se charge de communiquer avec des clients 
    /// </summary>
    public class Server
    {
        private const int URL_INDEX = 1;
        private const int RESPONSE_TYPE_INDEX = 0;
        private const int RESPONSE_DATA_INDEX = 1;
        private const int RECIPIENT_INDEX = 1;
        private const int ID_INDEX = 0;
        private IWebProtocol webSocket;
        private bool isRunning;
        private GameType gameType;
        private static ConcurrentDictionary<int, Game> customGames = new ConcurrentDictionary<int, Game>();
        private static ConcurrentDictionary<int, Game> matchmakingGames = new ConcurrentDictionary<int, Game>();
        private static ConcurrentDictionary<int, Lobby> lobbies = new ConcurrentDictionary<int, Lobby>();
        private static ConcurrentDictionary<string, IClient> connectedClients = new ConcurrentDictionary<string, IClient>();
        private static readonly Queue<IClient> waitingPlayers = new Queue<IClient>();

        private Interpreter interpreter;
        private GameManager gameManager;

        /// <summary>
        /// Renvoi ou modifie les parties personnalisées en cours
        /// </summary>
        public static ConcurrentDictionary<int, Game> CustomGames { get => customGames; set => customGames = value; }

        /// <summary>
        /// Renvoi ou modifie les parties de matchmaking en cours
        /// </summary>
        public static ConcurrentDictionary<int, Game> MatchmakingGames { get => matchmakingGames; set => matchmakingGames = value; }

        /// <summary>
        /// Renvoi ou modifie les lobbies en cours
        /// </summary>
        public static ConcurrentDictionary<int, Lobby> Lobbies { get => lobbies; set => lobbies = value; }

        /// <summary>
        /// File d'attente des joueurs en attente de matchmaking
        /// </summary>
        public static Queue<IClient> WaitingPlayers => waitingPlayers;

        /// <summary>
        /// Renvoi les clients connectés
        /// </summary>
        public static ConcurrentDictionary<string, IClient> ConnectedClients { get => connectedClients; }


        /// <summary>
        /// Constructeur de la classe Server
        /// </summary>
        public Server(string ip, int port)
        {
            this.webSocket = new Protocol.WebSocket(ip, port);
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
                        IClient client = new Client(tcp);
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

                                try
                                {// Ajoute le client à la liste des clients connectés après le handshake
                                    connectedClients.TryAdd(client.User.Name, client);
                                }
                                catch(Exception ex)
                                {
                                    // on ne fait rien en cas d'erreur, si un client se déconnecte pendant le handshake on ne veut pas que le serveur crash
                                }
                                
                                // Broadcaster la liste mise à jour des utilisateurs
                                this.BroadcastUserList();
                            }
                            else // Le message est un message chiffré
                            {
                                try
                                {
                                    this.TreatMessage(bytes, client, ref message, ref response);
                                }
                                catch (DisconnectionException ex) // Le message reçu est un message de déconnexion
                                {
                                    // À la déconnexion, retire le client de la liste des  et broadcaster la nouvelle liste
                                    connectedClients.TryRemove(client.User.Name, out _);
                                    this.DisconnectClient(client, ex, ref endOfCommunication);
                                    this.BroadcastUserList();
                                    
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
        private void ProceedHandshake(string message, IClient client, ref string response)
        {
            byte[] handshake = this.webSocket.BuildHandShake(message);
            response = Encoding.UTF8.GetString(handshake);
            client.SendMessage(handshake);
            ExtractTokenUserFromHandshake(message, client);
        }

        /// <summary>
        /// Extrait le token de la demande de l'url de demande de connexion entre le client et le server
        /// </summary>
        private void ExtractTokenUserFromHandshake(string message, IClient client)
        {
            string url = message.Split(" ")[URL_INDEX];
            var uri = new Uri($"http://{Environment.GetEnvironmentVariable("SERVER_IP")}:{Environment.GetEnvironmentVariable("SERVER_PORT")}{url}");
            string token = HttpUtility.ParseQueryString(uri.Query).Get("token");
            client.ChangeUser(gameManager.GetUserByToken(token));
        }

        /// <summary>
        /// Déconnecte un joueur de sa partie
        /// </summary>
        private void DisconnectClient(IClient client, DisconnectionException ex, ref bool endOfCommunication)
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
        }


        /// <summary>
        /// Traite le message reçu par le client en le décryptant et en le traitant selon son type
        /// </summary>
        /// <param name="bytes">Message chiffré reçu</param>
        /// <param name="client">Client qui a envoyé le message</param>
        /// <param name="message">Message déchiffré (modifié par référence)</param>
        /// <param name="response">Réponse à envoyer (modifiée par référence)</param>
        private void TreatMessage(byte[] bytes, IClient client, ref string message, ref string response)
        {
            message = DecryptAndParseMessage(bytes);
            ProcessMessage(client, ref message, ref response);
        }

        /// <summary>
        /// Décrypte et convertit le message reçu en chaîne de caractères
        /// </summary>
        private string DecryptAndParseMessage(byte[] bytes)
        {
            byte[] decryptedMessage = this.webSocket.DecryptMessage(bytes);
            return Encoding.UTF8.GetString(decryptedMessage);
        }

        /// <summary>
        /// Traite le message déchiffré et prépare la réponse appropriée
        /// </summary>
        /// <remarks>
        /// Cette méthode :
        /// 1. Détermine le type de jeu (custom ou matchmaking)
        /// 2. Interprète le message via l'interpréteur
        /// 3. Traite la réponse selon qu'il s'agit d'une action de jeu ou non
        /// </remarks>
        private void ProcessMessage(IClient client, ref string message, ref string response)
        {
            // Détermine le type de jeu
            if (message.Contains("custom"))
            {
                this.gameType = GameType.CUSTOM;
            }
            else if (message.Contains("matchmaking"))
            {
                this.gameType = GameType.MATCHMAKING;
            }

            // Interprète le message
            response = this.interpreter.Interpret(message, client, gameType);
            string[] data = response.Split("_");
            string responseType = data[RESPONSE_TYPE_INDEX];
            string responseData = data[RESPONSE_DATA_INDEX];

            string stringId = responseData.Split("-")[ID_INDEX];
            int idGame = Convert.ToInt32(stringId);
            byte[] responseBytes = this.webSocket.BuildMessage(responseData);
            if (this.IsGameAction(responseData))
            {
                this.HandleGameAction(client, responseType, responseData, responseBytes, idGame);
            }
            else
            {
                int idLobby = idGame;
                this.HandleNonGameAction(client, responseType, responseData, responseBytes, idLobby);
            }

            response = responseData;
        }

        /// <summary>
        /// Vérifie si le message correspond à une action de jeu
        /// </summary>
        /// <returns>true si c'est une action de jeu, false sinon</returns>
        private bool IsGameAction(string response)
        {
            return !response.Contains("Create") &&
                   !response.Contains("Timeout") &&
                   !response.Contains("Cancelled") &&
                   !response.Contains("Retry") &&
                   !response.Contains("Chat");
        }

        /// <summary>
        /// Gère les actions liées au jeu (placement de pierre, etc.)
        /// </summary>
        /// <remarks>
        /// Séquence de traitement :
        /// 1. Récupère la partie concernée
        /// 2. Démarre la partie si elle est pleine et non commencée
        /// 3. Diffuse le message si c'est un broadcast
        /// 4. Envoie la réponse au client si nécessaire
        /// </remarks>
        private void HandleGameAction(IClient client, string responseType, string responseData, byte[] responseBytes, int idGame)
        {
            Game game = this.gameType == GameType.CUSTOM ? customGames[idGame] : matchmakingGames[idGame];
            if (game.IsFull && !game.Started)
            {
                this.StartGame(game);
            }
            else if (responseType == "Broadcast")
            {
                this.BroadastMessageAsync(game, responseBytes);
            }
            if (responseType == "Send")
            {
                this.SendMessage(client, responseBytes);
            }
        }

        /// <summary>
        /// Gère les actions non liées au jeu (chat, matchmaking, etc.)
        /// </summary>
        /// <remarks>
        /// Types de messages traités :
        /// - Send : message direct au client
        /// - Broadcast : message à tous les clients d'un lobby
        /// - Private : message privé entre deux clients
        /// </remarks>
        private void HandleNonGameAction(IClient client, string responseType, string responseData, byte[] responseBytes, int idLobby)
        {
            if (responseType == "Send")
            {
                this.SendMessage(client, responseBytes);
            }
            else if (responseType == "Broadcast")
            {
                this.BroadcastCancelMessage(Server.Lobbies[idLobby], responseBytes);
            }
            else if (responseType.StartsWith("Private"))
            {
                string recipient = responseType.Split('-')[RECIPIENT_INDEX];
                if (Server.ConnectedClients.TryGetValue(recipient, out IClient recipientClient))
                {
                    byte[] messageBytes = this.webSocket.BuildMessage(responseData);
                    this.SendMessage(recipientClient, messageBytes);
                }
            }
        }

        /// <summary>
        /// Envoie un message à un client spécifique
        /// </summary>
        /// <param name="client">Client destinataire</param>
        /// <param name="bytes">Message à envoyer</param>
        private void SendMessage(IClient client, byte[] bytes)
        {
            if (client != null)
            {
                client.SendMessage(bytes);
            }
        }

        /// <summary>
        /// Diffuse un message aux deux joueurs d'une partie et vérifie si la partie est terminée
        /// </summary>
        /// <remarks>
        /// La méthode est asynchrone car elle doit attendre la vérification de fin de partie
        /// </remarks>
        private async Task BroadastMessageAsync(Game game, byte[] bytes)
        {
            this.SendMessage(game.Player1, bytes);
            this.SendMessage(game.Player2, bytes);

            // Vérifie si la partie est terminée
            if (await game.TestWinAsync())
            {
                // Si la partie est terminée, gérer la fin (en parallèle des tâches BDD)
                this.handleGameEnd(game);
            }
        }

        /// <summary>
        /// Diffuse un message d'annulation aux joueurs d'un lobby et supprime le lobby (diffusé lors de l'annulation de matchmaking)
        /// </summary>
        private void BroadcastCancelMessage(Lobby lobby, byte[] bytes)
        {
            this.SendMessage(lobby.Player1, bytes);
            this.SendMessage(lobby.Player2, bytes);
            Server.Lobbies.TryRemove(lobby.Id, out _);
        }


        /// <summary>
        /// Démarre une partie en récupérant les joueurs et en envoyant le nom de l'adversaire à chaque joueur ainsi que l'état du plateau
        /// </summary>
        private void StartGame(Game game)
        {
            string gameBoard = game.StringifyGameBoard();
            byte[] startP1 = this.webSocket.BuildMessage($"{game.Id}-Start-{game.Player2.User.Name}-{gameBoard}"); // Envoi du nom du joueur à son adversaire
            byte[] startP2 = this.webSocket.BuildMessage($"{game.Id}-Start-{game.Player1.User.Name}-{gameBoard}"); // Envoi du nom du joueur à son adversaire
            this.SendMessage(game.Player1, startP1);
            this.SendMessage(game.Player2, startP2);
            game.Start();
        }

        /// <summary>
        /// Gère la fin de partie en récupérant le score final des deux joueurs et en renvoyant le gagnant aux deux joueurs 
        /// </summary>
        private void handleGameEnd(Game game)
        {
            (float , float) scores = game.GetScore();
            float scorePlayer1 = scores.Item1;
            float scorePlayer2 = scores.Item2;
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
        /// Envoi la liste des utilisateurs à tous les utilisateurs connectés
        /// </summary>
        private void BroadcastUserList()
        {
            string userListMessage = "0-UserList-" + string.Join(",", connectedClients.Keys);
            byte[] userListBytesMessage = this.webSocket.BuildMessage(userListMessage);
            
            foreach (var client in connectedClients.Values)
            {
                this.SendMessage(client,userListBytesMessage);
            }
        }

    }

}
