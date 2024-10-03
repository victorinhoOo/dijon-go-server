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
