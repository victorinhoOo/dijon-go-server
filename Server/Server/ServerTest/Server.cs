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
        private char[] buffer;
        private Dictionary<string, Dictionary<string, Client>> games;
        private bool isRunning;

        public Server()
        {
            this.listener = new TcpListener(IPAddress.Parse("10.211.55.3"), 7000); //10.211.55.3
            this.games = new Dictionary<string, Dictionary<string, Client>>();
            this.buffer = new char[2048];
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
                        StreamReader streamReader = new StreamReader(tcp.GetStream());
                        StreamWriter streamWriter = new StreamWriter(tcp.GetStream()) { AutoFlush = true };
                        this.Handshake(tcp, streamReader, streamWriter);
                        string message = "";
                        bool endOfCommunication = false;
                        
                        message = streamReader.ReadLine();
                        Console.WriteLine("Received: {0}", message);

                    });
                    thread.Start();
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur: " + ex.Message);
                }
            }
        }

        public void Handshake(TcpClient client, StreamReader streamReader, StreamWriter streamWriter)
        {
            Console.WriteLine("new client connected");
            string data = "";
            string message="";
            while((data = streamReader.ReadLine()) != "")
            {
                message += $"{data}\r\n" ;
            }
            Console.WriteLine("Received: {0}", message);


            string secretKey = "";
            foreach (string line in message.Split("\r\n"))
            {
                if (line.StartsWith("Sec-WebSocket-Key"))
                {
                    secretKey = line.Split(": ")[1].Trim();
                }
            }

            string concatenated = secretKey + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
            SHA1 sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(Encoding.ASCII.GetBytes(concatenated));

            string hash = Convert.ToBase64String(hashBytes);

            string response = "HTTP/1.1 101 Switching Protocols\r\n" + "Upgrade: websocket\r\n" + "Connection: Upgrade\r\n" + $"Sec-WebSocket-Accept: {hash}\r\n";
            
            streamWriter.WriteLine(response);

            Console.WriteLine("Sent: {0}", response);
        }
    }
}
