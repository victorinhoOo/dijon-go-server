using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ServerTest
{
    public class Server
    {
        private TcpListener listener;
        private Dictionary<string, Dictionary<string, Client>> games;
        private bool isRunning;

        public Server()
        {
            this.listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7000); //10.211.55.3
            this.games = new Dictionary<string, Dictionary<string, Client>>();
        }

        public void Start()
        {
            this.listener.Start();
            this.isRunning = true;
            Console.WriteLine("Server Started");

            while (isRunning)
            {
                try
                { 
                    Thread thread = new Thread(() => {
                        TcpClient tcp = this.listener.AcceptTcpClient();

                        Byte[] bytes = new Byte[256];
                        String data = null;

                        Client client = new Client(tcp);
                        if (this.games.Count > 0) // si il y a au moins une partie de créée 
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
                                
                        Console.WriteLine("new client connected"); 
                        NetworkStream stream = tcp.GetStream();
                        int i;
                        while((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                            Console.WriteLine("Received: {0}", data);
                            string secretKey = "";
                            foreach(string line in data.Split("\r\n"))
                            {
                                if (line.StartsWith("Sec-WebSocket-Key"))
                                {
                                    secretKey = line.Split(": ")[1].Trim();
                                }
                            }

                            string concatenated = secretKey + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
                            SHA1 sha1 = SHA1.Create();
                            byte[] hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(concatenated));

                            string hash = Convert.ToBase64String(hashBytes);

                            string response = "HTTP/1.1 101 Switching Protocols\r\n" + "Upgrade: websocket\r\n" + "Connection: Upgrade\r\n" + $"Sec-WebSocket-Accept: {hash}\r\n\r\n";


                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(response);

                         
                            stream.Write(msg, 0, msg.Length);
                            Console.WriteLine("Sent: {0}", response);
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
    }
}
