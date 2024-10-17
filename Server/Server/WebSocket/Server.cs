using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
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
            this.webSocket = new WebSocket("10.211.55.3", 7000); //10.211.55.3
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
            foreach(var game in Server.Games)
            {
                if(game.Value.Player1 == client)
                {
                    game.Value.Player1 = null;
                }
                else if(game.Value.Player2 == client)
                {
                    game.Value.Player2 = null;
                }
            }
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
            if (game.IsFull)
            {
                this.StartGame(game);
            }
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

        public void StartGame(Game game)
        {
            string p1 = this.interpreter.getUsernameByToken(game.Player1.Token);
            string p2 = this.interpreter.getUsernameByToken(game.Player2.Token);
            byte[] startP1 = this.webSocket.BuildMessage($"{game.Id}/Start:{p2}");
            byte[] startP2 = this.webSocket.BuildMessage($"{game.Id}/Start:{p1}");
            this.SendMessage(game.Player1, startP1);
            this.SendMessage(game.Player2, startP2);
        }
    }

}
