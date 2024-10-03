using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
            this.listener = new TcpListener(IPAddress.Parse("10.211.55.3"), 7000);
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
                        Client client = new Client(tcp);
                        if (this.games.Count > 0)
                        {
                            foreach(string key in this.games.Keys)
                            {
                                if (this.games[key].Count() < 2)
                                {
                                    int count = this.games[key].Count();
                                    this.games[key][$"player{count}"] = client;
                                }
                            }
                        }
                        if (this.games.ContainsKey($"game{this.games.Count()}"))
                        {
                            Dictionary<string, Client> game = this.games[$"game{this.games.Count()}"];
                            if (game.Count() < 2)
                            {
                                game[$"player{game.Count()}"] = client;
                            }
                        }
                        else
                        {
                            Dictionary<string, Client> game = new Dictionary<string, Client>();
                            game.Add("player0", client);
                            this.games[$"game{this.games.Count()}"] = game;
                        }
                        Console.WriteLine("new client connected"); 
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
