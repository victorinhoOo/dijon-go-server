using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace WebSocket
{
    /// <summary>
    /// Classe qui se charge de communiquer avec des clients 
    /// </summary>
    public class Server
    {
        private IWebProtocol webSocket;
        private bool isRunning;
        private static ConcurrentDictionary<int, Game> games = new ConcurrentDictionary<int, Game>();
        private Interpreter interpreter;

        public static ConcurrentDictionary<int, Game> Games { get => games; set => games = value; }

        public Server()
        {
            this.webSocket = new WebSocket("127.0.0.1", 7000); //10.211.55.3
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

                        /*String data = null;

                        Client client = new Client(tcp);
                        /*if (this.games.Count > 0) // si il y a au moins une partie de créée 
                        {
                            Dictionary<string, Client> game = this.games[$"game{this.games.Count()}"]; // on récupère la dernière partie créée
                            if(game.Count < 2)
                            {
                                game["player2"] = client;
                            }
                            else
                            {
                                Dictionary<string, Client> newGame = new Dictionary<string, Client>(); // création d'une nouvelle partie
                                this.games[$"game{this.games.Count()+1}"] = newGame; // ajout de la partie créée au dictionnaire des parties
                                newGame["player1"] = client; // ajout du joueur à la partie fraichement créée
                            }
                        }
                        else
                        {
                            Dictionary<string, Client> newGame = new Dictionary<string, Client>(); // création d'une nouvelle partie
                            this.games[$"game{this.games.Count() + 1}"] = newGame; // ajout de la partie créée au dictionnaire des parties
                            newGame["player1"] = client; // ajout du joueur à la partie fraichement créée
                        }
                        this.Handshake(tcp, stream);
                        int i;
                        while((i = stream.Read(this.buffer, 0, this.buffer.Length)) != 0)
                        {
                            data = Encoding.ASCII.GetString(this.buffer, 0, i);
                            Console.WriteLine("Received: {0}", data);
                        }*/

                    });
                    thread.Start();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur: " + ex.Message);
                }
            }
        }
        private bool MessageIsHandshakeRequest(string message)
        {
            return Regex.IsMatch(message, "^GET");
        }

        private void ProceedHandshake(string message, Client client, ref string response)
        {
            byte[] handshake = this.webSocket.BuildHandShake(message);
            response = Encoding.UTF8.GetString(handshake);
            client.SendMessage(handshake);
        }

        private void DisconnectClient(Client client, DisconnectionException ex, ref bool endOfCommunication)
        {
            byte[] deconnectionBytes = this.webSocket.BuildDeconnection(ex.Code);
            client.SendMessage(deconnectionBytes);
            Console.WriteLine(ex.Message + "\n");
            endOfCommunication = true; // Fin de la communication
        }

        private void TreatMessage(byte[] bytes, Client client, ref string message, ref string response)
        {
            byte[] decryptedMessage = this.webSocket.DecryptMessage(bytes);
            message = Encoding.UTF8.GetString(decryptedMessage);
            response = this.interpreter.Interpret(message, client);

            string responseType = response.Split("_")[0];
            string responseData = response.Split("_")[1];

            int idGame = Convert.ToInt32(responseData.Split("/")[0]);
            byte[] responseBytes = this.webSocket.BuildMessage(responseData);
            Game game = games[idGame];
            if (responseType == "Send")
            {
                this.SendMessage(client, responseBytes);
            }
            else if(responseType == "Broadcast")
            {
                this.BroadastMessage(game, responseBytes);
            }
            response = responseData;
        }

        private void SendMessage(Client client, byte[] bytes)
        {
            if(client != null)
            {
                client.SendMessage(bytes);
            }
        }

        private void BroadastMessage(Game game, byte[] bytes)
        {
            this.SendMessage(game.Player1, bytes);
            this.SendMessage(game.Player2, bytes);
        }
    }

}
