using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
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
        private Dictionary<string, Dictionary<string, Client>> games;
        private bool isRunning;

        public Server()
        {
            this.webSocket = new WebSocket("10.211.55.3",7000);
            this.games = new Dictionary<string, Dictionary<string, Client>>();
        }


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
                        bool endOfCommunication = false;
                        while (!endOfCommunication)
                        {
                            byte[] bytes = client.ReceiveMessage();
                            string message = Encoding.UTF8.GetString(bytes);
                            string response = ""; 

                            if (Regex.IsMatch(message, "^GET")) // test si le message reçu est une demande de handshake
                            {
                                byte[] handshake = this.webSocket.BuildHandShake(message);
                                response = Encoding.UTF8.GetString(handshake);
                                client.SendMessage(handshake);
                            }
                            else
                            {
                                try
                                {
                                    byte[] decryptedMessage = this.webSocket.DecryptMessage(bytes);
                                    message = Encoding.UTF8.GetString(decryptedMessage);
                                    response = "Hello World";
                                    byte[] responseBytes = this.webSocket.BuildMessage(response);
                                    client.SendMessage(responseBytes);
                                }
                                catch(DeconnectionException ex)
                                {
                                    byte[] deconnectionBytes = this.webSocket.BuildDeconnection(ex.Code);
                                    client.SendMessage(deconnectionBytes);
                                    Console.WriteLine(ex.Message + "\n");
                                    endOfCommunication = true;
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
    }
}
