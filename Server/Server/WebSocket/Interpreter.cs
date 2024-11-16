﻿using GoLogic;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using WebSocket.Model;
using WebSocket.Model.DAO;

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
        public string Interpret(string message, Client client)
        {

            int idGame = Convert.ToInt32(message.Split("/")[0]);
            message = message.Split("/")[1];
            string action = message.Split(":")[0]; // action à effectuer
            string type = ""; // type de réponse (send ou broadcast)
            string response = "";
            switch (action)
            {
                case "Stone": PlaceStone(client, idGame, message.Split(':')[1], ref response, ref type); break;
                case "Create": CreateGame(client, message, ref response, ref type); break;
                case "Join": JoinGame(client, message, idGame, ref response, ref type); break;
                case "Matchmaking": Matchmaking(client, message, ref response, ref type); break;
                case "Skip": Skip(client, idGame, ref response, ref type); break;

            }
            return type + response;

        }


        /// <summary>
        /// le joueur place une pierre sur le plateau
        /// </summary>
        private void PlaceStone(Client player, int idGame, string coordinates, ref string response, ref string type)
        {
            if (idGame != 0)
            {
                Game game = Server.Games[idGame];
                if (game.CurrentTurn == player) // si c'est le tour du joueur
                {
                    try
                    {
                        int x = Convert.ToInt32(coordinates.Split("-")[0]);
                        int y = Convert.ToInt32(coordinates.Split("-")[1]);

                        game.PlaceStone(x, y); // pose de la pierre
                        (int, int) score = game.GetScore(); // récupération du score
                        game.ChangeTurn(); // changement de tour
                        response = $"{idGame}/{game.StringifyGameBoard()}|{score.Item1};{score.Item2}";
                        type = "Broadcast_";
                    }
                    catch (Exception e)
                    {
                        response = $"{idGame}/Error:{e.Message}";
                        type = "Send_";
                    }
                }
                else // si ce n'est pas le tour du joueur
                {
                    response = $"{idGame}/Not your turn";
                    type = "Send_";
                }
            }
        }


        /// <summary>
        /// Le joueur créé une partie 
        /// </summary>
        private void CreateGame(Client client, string message, ref string response, ref string type)
        {
            int id = Server.Games.Count + 1; // Génération de l'id de la partie
            Game newGame = new Game();
            newGame.AddPlayer(client);
            Server.Games[id] = newGame;
            gameDAO.InsertGame(newGame); // Ajout de la partie dans le dictionnaire des parties
            client.Token = message.Split(":")[1];
            Server.Games[id].Player1 = client; // Ajout du client en tant que joueur 1
            response = $"{id}/"; // Renvoi del'id de la partie créée
            type = "Send_";

        }


        /// <summary>
        /// Le joueur rejoint une partie
        /// </summary>
        private void JoinGame(Client client, string message, int idGame, ref string reponse, ref string type)
        {
            client.Token = message.Split(":")[1]; // Récupération du token du joueur afin d'afficher son pseudo et sa photo de profil
            Server.Games[idGame].AddPlayer(client); // Ajout du client en tant que joueur 2
            gameDAO.DeleteGame(idGame); // Suppression de la partie de la liste des parties disponibles
            reponse = $"{idGame}/"; // Renvoi de l'id de la partie rejointe 
            type = "Send_";
        }

        private void Matchmaking(Client client, string message, ref string response, ref string type)
        {
            // Ajoute le joueur à la file d'attente
            Server.WaitingPlayers.Enqueue(client);

            // Si deux joueurs ou plus sont en attente, démarrez une partie
            if (Server.WaitingPlayers.Count >= 2)
            {
                Client player1 = Server.WaitingPlayers.Dequeue();
                Client player2 = Server.WaitingPlayers.Dequeue();

                // Créé une nouvelle partie pour eux
                int id = Server.Games.Count + 1;
                Game newGame = new Game();
                newGame.AddPlayer(player1);
                newGame.AddPlayer(player2);

                Server.Games[id] = newGame;

                // Définir les joueurs
                newGame.Player1 = player1;
                newGame.Player2 = player2;
                // todo
                response = $"{id}/";
                type = "Broadcast_";
            }
            else
            {
                // Informer le joueur qu'il est en attente d'un adversaire
                response = "Waiting";
                type = "Send_";
            }
        }


        /// <summary>
        /// Le joueur passe son tour
        /// </summary>
        private void Skip(Client client, int idGame, ref string response, ref string type)
        {
            Game game = Server.Games[idGame];
            if(client == game.CurrentTurn)
            {
                game.SkipTurn();
                type = "Broadcast_";
                response = $"{idGame}/Turn skipped";
            }
            else
            {
                response = $"{idGame}/Not your turn";
                type = "Send_";
            }
            
        }


        /// <summary>
        /// Récuppérer le pseudo du joueur à partir de son token
        /// </summary>
        /// <param name="token">token du joueur</param>
        /// <returns>le pseudo du joueur</returns>
        public string GetUsernameByToken(string token)
        {
            return userDAO.GetUsernameByToken(token);
        }

    }
}
