using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                    Thread thread = new Thread(() =>
                    {
                        TcpClient tcp = this.listener.AcceptTcpClient();
                        NetworkStream stream = tcp.GetStream();

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

                        this.Handshake(tcp, stream);
                        byte[] buffer = new byte[1024];
                        int i;
                        string data;
                        while ((i = stream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            bool fin = (buffer[0] & 0b10000000) != 0, mask = (buffer[1] & 0b10000000) != 0; // must be true, "All messages from the client to the server have this bit set"
                            int opcode = buffer[0] & 0b00001111; // expecting 1 - text message
                            ulong offset = 2, msglen = buffer[1] & (ulong)0b01111111;

                            if (msglen == 126)
                            {
                                // bytes are reversed because websocket will print them in Big-Endian, whereas
                                // BitConverter will want them arranged in little-endian on windows
                                msglen = BitConverter.ToUInt16(new byte[] { buffer[3], buffer[2] }, 0);
                                offset = 4;
                            }
                            else if (msglen == 127)
                            {
                                // To test the below code, we need to manually buffer larger messages — since the NIC's autobuffering
                                // may be too latency-friendly for this code to run (that is, we may have only some of the bytes in this
                                // websocket frame available through client.Available).
                                msglen = BitConverter.ToUInt64(new byte[] { buffer[9], buffer[8], buffer[7], buffer[6], buffer[5], buffer[4], buffer[3], buffer[2] }, 0);
                                offset = 10;
                            }

                            if (msglen == 0)
                            {
                                Console.WriteLine("msglen == 0");
                            }
                            else if (mask)
                            {
                                byte[] decoded = new byte[msglen];
                                byte[] masks = new byte[4] { buffer[offset], buffer[offset + 1], buffer[offset + 2], buffer[offset + 3] };
                                offset += 4;

                                for (ulong j = 0; j < msglen; ++j)
                                    decoded[j] = (byte)(buffer[offset + j] ^ masks[j % 4]);

                                string text = Encoding.UTF8.GetString(decoded);
                                Console.WriteLine("Received : {0}", text);
                                byte[] response = new byte[] { 129, 2, 79, 75 };
                                stream.Write(response);
                                byte[] message = new byte[2];
                                Array.Copy(response, response.Length - 2, message, 0, 2);
                                Console.WriteLine("Sent : {0}", Encoding.UTF8.GetString(message));
                            }
                            else
                                Console.WriteLine("mask bit not set");

                            Console.WriteLine();
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

        public void Handshake(TcpClient client, NetworkStream stream)
        {
            Console.WriteLine("new client connected");
            byte[] buffer = new byte[1024];
            stream.Read(buffer, 0, buffer.Length);
            string data = Encoding.UTF8.GetString(buffer);
            
            Console.WriteLine("Received: {0}", data);

            string secretKey = "";
            foreach (string line in data.Split("\r\n"))
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

            string response = "HTTP/1.1 101 Switching Protocols\r\n" + "Upgrade: websocket\r\n" + "Connection: Upgrade\r\n" + $"Sec-WebSocket-Accept: {hash}\r\n\r\n";
            
            stream.Write(Encoding.UTF8.GetBytes(response));

            Console.WriteLine("Sent: {0}", response);
        }
    }
}
