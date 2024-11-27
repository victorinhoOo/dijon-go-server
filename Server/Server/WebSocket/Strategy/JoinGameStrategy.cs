using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Model.DAO;

namespace WebSocket.Strategy
{
    /// <summary>
    /// Le joueur rejoint une partie
    /// </summary>
    public class JoinGameStrategy : IStrategy
    {

        private IGameDAO gameDAO;
        public JoinGameStrategy()
        {
            this.gameDAO = new GameDAO();
        }
        public void execute(Client player, string[] data, string gameType, ref string response, ref string type)
        {
            string stringId = data[0];
            int idGame = Convert.ToInt16(stringId);
            if (gameType == "custom")
            {

                player.User.Token = data[2]; // Récupération du token du joueur afin d'afficher son pseudo et sa photo de profil
                Server.Games[idGame].AddPlayer(player); // Ajout du client en tant que joueur 2
                this.gameDAO.DeleteGame(idGame); // Suppression de la partie de la liste des parties disponibles
                response = $"{idGame}-"; // Renvoi de l'id de la partie rejointe 
                type = "Send_";
            }
            else if (gameType == "matchmaking")
            {
                player.User.Token = data[2];
                Server.MatchmakingGames[idGame].AddPlayer(player);
                response = $"{idGame}-"; // Renvoi de l'id de la partie rejointe 
                type = "Send_";
            }
        }
    }
}
