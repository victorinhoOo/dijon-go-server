using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket.Strategy
{
    /// <summary>
    /// Stratégie de matchmaking permettant de mettre en relation deux joueurs pour une partie.
    /// Cette classe gère la file d'attente et la création/jointure des parties multijoueurs.
    /// </summary>
    public class MatchmakingStrategy : IStrategy
    {
        /// <summary>
        /// File contenant les joueurs en recherche de matchmaking
        /// </summary>
        private static readonly Queue<Client> waitingPlayers = new Queue<Client>();

        /// <summary>
        /// Temps en secondes au bout duquel le matchmaking s'annule 
        /// </summary>
        const int TIMEOUT_SECONDS = 20;

        /// <summary>
        /// Exécute la logique de matchmaking pour un joueur.
        /// </summary>
        /// <param name="player">Le client/joueur qui demande le matchmaking</param>
        /// <param name="data">Données additionnelles (non utilisées)</param>
        /// <param name="gameType">Type de partie demandée (ici "matchmaking")</param>
        /// <param name="response">Réponse à envoyer au client (modifiée par référence)</param>
        /// <param name="type">Type de réponse à envoyer (modifié par référence)</param>
        public void Execute(Client player, string[] data, string gameType, ref string response, ref string type)
        {
            waitingPlayers.Enqueue(player);
            int initialNbMatchmakingGames = Server.MatchmakingGames.Count();
            bool timeout = false;

            Client player1 = waitingPlayers.Peek();
            if (player == player1) // Le joueur qui rejoint est le premier joueur
            {
                // Attente du second joueur
                timeout = WaitForCondition(() => waitingPlayers.Count >= 2);
                if (!timeout)
                {
                    response = "0-Create-matchmaking";
                }
            }
            else // le joueur qui rejoint est le deuxième joueur
            {
                // Attente de la création de la partie
                timeout = WaitForCondition(() => Server.MatchmakingGames.Count > initialNbMatchmakingGames);
                if (!timeout)
                {
                    waitingPlayers.Dequeue();
                    waitingPlayers.Dequeue();
                    string idGame = Server.MatchmakingGames.Count().ToString();
                    response = $"{idGame}-Join-matchmaking";
                }
            }

            if (timeout) // Si on a attendu mais que personne n'a rejoint au bout de TIMEOUT_SECONDS
            {
                waitingPlayers.Dequeue();
                response = "0-Timeout";
            }
            type = "Send_";
        }

        /// <summary>
        /// Attend qu'une condition soit remplie pendant une durée maximale définie par TIMEOUT_SECONDS.
        /// </summary>
        /// <param name="condition">Délégué représentant la condition à vérifier périodiquement</param>
        /// <returns>
        /// true si le délai d'attente a expiré avant que la condition ne soit remplie,
        /// false si la condition a été remplie avant l'expiration du délai
        /// </returns>
        private bool WaitForCondition(Func<bool> condition)
        {
            bool timeout = false;
            DateTime startTime = DateTime.Now;
            while (!condition() && !timeout)
            {
                if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS)
                {
                    timeout = true;
                }
                Thread.Sleep(100);
            }
            return timeout;
        }
    }
}
