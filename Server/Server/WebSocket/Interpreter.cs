using GoLogic;
using System.Globalization;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using WebSocket.Model;
using WebSocket.Model.DAO;
using WebSocket.Model.DTO;

namespace WebSocket
{
    /// <summary>
    /// Interprète les messages reçus par le serveur
    /// </summary>
    public class Interpreter
    {
        private IGameDAO gameDAO;
        private IUserDAO userDAO;

        /// <summary>
        /// Constructeur de la classe Interpreter
        /// </summary>
        public Interpreter()
        {
            gameDAO = new GameDAO();
            userDAO = new UserDAO();
        }

        /// <summary>
        /// Réagit au message reçu par le serveur
        /// </summary>
        /// <param name="message">Message reçu</param>
        /// <param name="client">client qui expédie le message</param>
        /// <returns>la réponse du serveur au client</returns>
        public string Interpret(string message, Client client, string gameType)
        {
            string[] data = message.Split("-");
            string action = data[1]; // action à effectuer
            string type = ""; // type de réponse (send ou broadcast)
            string response = "";
            switch (action)
            {
                case "Stone": PlaceStone(client, data, gameType, ref response, ref type); break;
                case "Create": CreateGame(client, data, ref response, ref type); break;
                case "Join": JoinGame(client, data, ref response, ref type); break;
                case "Matchmaking": Matchmaking(client, ref response, ref type); break;
                case "Skip": Skip(client, data, gameType, ref response, ref type); break;
            }           
            return type + response;

        }


        /// <summary>
        /// le joueur place une pierre sur le plateau
        /// </summary>
        private void PlaceStone(Client player, string[] data, string gameType, ref string response, ref string type)
        {
            string stringId = data[0];
            int idGame = Convert.ToInt16(stringId);
            if (idGame != 0)
            {
                Game game = null;
                if(gameType == "custom")
                {
                    game = Server.Games[idGame];
                }
                else if (gameType == "matchmaking")
                {
                    game = Server.MatchmakingGames[idGame];
                }
                if (game.CurrentTurn == player) // si c'est le tour du joueur
                {
                    try
                    {
                        int x = Convert.ToInt32(data[2]);
                        int y = Convert.ToInt32(data[3]);

                        string timeRemaining = game.PlaceStone(x, y); // pose de la pierre
                        (int capturedBlackStones, int capturedWhiteStones) = game.GetCapturedStone(); // récupération des pierres capturées
                        game.ChangeTurn(); // changement de tour
                        response = $"{idGame}-{game.StringifyGameBoard()}-{capturedBlackStones}-{capturedWhiteStones}-{timeRemaining.Split(',')[0]}";
                        type = "Broadcast_";
                    }
                    catch (Exception e)
                    {
                        response = $"{idGame}-Error-{e.Message}";
                        type = "Send_";
                    }
                }
                else // si ce n'est pas le tour du joueur
                {
                    response = $"{idGame}-Not your turn";
                    type = "Send_";
                }
            }
        }


        /// <summary>
        /// Le joueur créé une partie 
        /// </summary>
        private void CreateGame(Client client, string[] data, ref string response, ref string type)
        {
            int size = Convert.ToInt16(data[3]);
            string rule = data[4];
            string gameType = data[5];
            if(gameType == "custom") // la partie est personnalisée
            {
                int id = Server.Games.Count + 1; // Génération de l'id de la partie
                Game newGame = new Game(size, rule);
                newGame.AddPlayer(client);
                Server.Games[id] = newGame;
                gameDAO.InsertGame(newGame); // Ajout de la partie dans le dictionnaire des parties
                client.User.Token = data[2];
                Server.Games[id].Player1 = client; // Ajout du client en tant que joueur 1
                response = $"{id}-"; // Renvoi de l'id de la partie créée
                type = "Send_";
            }
            else if (gameType == "matchmaking")
            {
                int id = Server.MatchmakingGames.Count + 1; // Génération de l'id de la partie
                Game newGame = new Game(size, rule);
                newGame.AddPlayer(client);
                Server.MatchmakingGames[id] = newGame;
                client.User.Token = data[2];
                Server.MatchmakingGames[id].Player1 = client;
                response = $"{id}-"; // Renvoi del'id de la partie créée
                type = "Send_";
            }

        }


        /// <summary>
        /// Le joueur rejoint une partie
        /// </summary>
        private void JoinGame(Client client, string[] data, ref string reponse, ref string type)
        {
            string stringId = data[0];
            int idGame = Convert.ToInt16(stringId);
            string gameType = data[3];
            if(gameType == "custom")
            {

                client.User.Token = data[2]; // Récupération du token du joueur afin d'afficher son pseudo et sa photo de profil
                Server.Games[idGame].AddPlayer(client); // Ajout du client en tant que joueur 2
                gameDAO.DeleteGame(idGame); // Suppression de la partie de la liste des parties disponibles
                reponse = $"{idGame}-"; // Renvoi de l'id de la partie rejointe 
                type = "Send_";
            }
            else if (gameType == "matchmaking")
            {
                client.User.Token = data[2];
                Server.MatchmakingGames[idGame].AddPlayer(client);
                reponse = $"{idGame}-"; // Renvoi de l'id de la partie rejointe 
                type = "Send_";
            }
        }
        /// <summary>
        /// Le joueur se met en recherche de partie
        /// </summary>
        private void Matchmaking(Client client, ref string response, ref string type)
        {
            Server.WaitingPlayers.Enqueue(client);
            int nbMatchmakingGames = Server.MatchmakingGames.Count();
            DateTime startTime = DateTime.Now;
            const int TIMEOUT_SECONDS = 5;

            Client player1 = Server.WaitingPlayers.Peek();
            if (client == player1)
            {
                // Le premier joueur attend avec un délai qui permet de vérifier périodiquement
                // si un second joueur est arrivé
                while (Server.WaitingPlayers.Count < 2) 
                {
                    if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS)
                    {
                        Server.WaitingPlayers.Dequeue(); // Retire le joueur de la file d'attente
                        response = "0-Timeout";
                        type = "Send_";
                        return;
                    }
                    Thread.Sleep(100);
                }
                response = "0-Create-matchmaking";
                type = "Send_";
            }
            else 
            {
                // Le deuxième joueur a rejoint la file
                while (Server.MatchmakingGames.Count == nbMatchmakingGames)
                {
                    if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS)
                    {
                        Server.WaitingPlayers.Dequeue(); // Retire le joueur de la file d'attente
                        response = "0-Timeout";
                        type = "Send_";
                        return;
                    }
                    Thread.Sleep(100);
                }              
                Server.WaitingPlayers.Dequeue();
                Server.WaitingPlayers.Dequeue();
                string idGame = (Server.MatchmakingGames.Count()).ToString();
                response = $"{idGame}-Join-matchmaking";
                type = "Send_";
            }
        }


        /// <summary>
        /// Le joueur passe son tour
        /// </summary>
        private void Skip(Client client, string[] data, string gameType, ref string response, ref string type)
        {
            string stringId = data[0];
            int idGame = Convert.ToInt16(stringId);
            Game game = null;
            if (gameType == "custom")
            {
                game = Server.Games[idGame];
            }
            else if (gameType == "matchmaking")
            {
                game = Server.MatchmakingGames[idGame];
            }
            if (gameType == "matchmaking")
            {
                game = Server.MatchmakingGames[idGame];
            }
            if(client == game.CurrentTurn)
            {
                game.SkipTurn();
                type = "Broadcast_";
                response = $"{idGame}-Turn skipped";
            }
            else
            {
                response = $"{idGame}-Not your turn";
                type = "Send_";
            }
            
        }

    }
}
