using MySqlX.XDevAPI;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Model;
using WebSocket.Model.DAO;
using WebSocket.Strategy.Enumerations;

namespace WebSocket.Strategy
{
    /// <summary>
    /// Stratégie de matchmaking permettant de mettre en relation deux joueurs pour une partie.
    /// Cette classe gère la file d'attente et la création/jointure des parties multijoueurs.
    /// </summary>
    public class MatchmakingStrategy : IStrategy
    {

        /// <summary>
        /// Temps en secondes au bout duquel le matchmaking s'annule 
        /// </summary>
        private const int TIMEOUT_SECONDS = 2000000;

        public MatchmakingStrategy()
        {
        }

        /// <summary>
        /// Exécute la logique de matchmaking pour un joueur.
        /// </summary>
        /// <param name="player">Le client/joueur qui demande le matchmaking</param>
        /// <param name="data">Données additionnelles (non utilisées)</param>
        /// <param name="gameType">Type de partie demandée (ici "matchmaking")</param>
        /// <param name="response">Réponse à envoyer au client (modifiée par référence)</param>
        /// <param name="type">Type de réponse à envoyer (modifié par référence)</param>
        public void Execute(IClient player, string[] data, GameType gameType, ref string response, ref string type)
        {
            Server.WaitingPlayers.Enqueue(player);
            int initialNbMatchmakingGames = Server.MatchmakingGames.Count();
            MatchmakingState state = MatchmakingState.RETRY;

            int idLobby = initialNbMatchmakingGames + 1;
            if (!Server.Lobbies.ContainsKey(idLobby))
            {
                Server.Lobbies[idLobby] = new Lobby(idLobby);
            }

            if (Server.WaitingPlayers.Count == 1) // Le joueur qui rejoint est le premier joueur
            {
                HandleFirstPlayer(player, idLobby, ref response, ref state);
            }
            else // le joueur qui rejoint est le deuxième joueur
            {
                HandleSecondPlayer(player, idLobby, initialNbMatchmakingGames, ref response, ref state);
            }

            HandleState(state, ref response);
            type = "Send_";
        }

        private void HandleFirstPlayer(IClient player, int idLobby, ref string response, ref MatchmakingState state)
        {
            Server.Lobbies[idLobby].Player1 = player;
            // Attente du second joueur
            state = WaitForCondition(() => Server.WaitingPlayers.Count >= 2, () => !Server.Lobbies.ContainsKey(idLobby));
            if (state == MatchmakingState.OK)
            {
                IClient opponement = Server.Lobbies[idLobby].Player2;
                string opponentUsername = opponement.User.Name;
                int opponentElo = opponement.User.Elo;
                Server.WaitingPlayers.Dequeue();
                response = $"0-Create-matchmaking-{idLobby}-{opponentUsername}-{opponentElo}";
            }
        }

        private void HandleSecondPlayer(IClient player, int idLobby, int initialNbMatchmakingGames, ref string response, ref MatchmakingState state)
        {
            Server.Lobbies[idLobby].Player2 = player;
            // Attente de la création de la partie
            state = WaitForCondition(() => Server.MatchmakingGames.Count > initialNbMatchmakingGames, () => !Server.Lobbies.ContainsKey(idLobby));
            if (state == MatchmakingState.OK)
            {
                IClient opponement = Server.Lobbies[idLobby].Player1;
                string opponentUsername = opponement.User.Name;
                int opponentElo = opponement.User.Elo;
                Server.WaitingPlayers.Dequeue();
                string idGame = Server.MatchmakingGames.Count().ToString();
                response = $"{idGame}-Join-matchmaking-{idLobby}-{opponentUsername}-{opponentElo}";
            }
        }

        private void HandleState(MatchmakingState state, ref string response)
        {
            if (state == MatchmakingState.RETRY)
            {
                Server.WaitingPlayers.Dequeue();
                response = "0-Retry";
            }
            else if (state == MatchmakingState.TIMEOUT) // Si on a attendu mais que personne n'a rejoint au bout de TIMEOUT_SECONDS
            {
                Server.WaitingPlayers.Dequeue();
                response = "0-Timeout";
            }
        }

        /// <summary>
        /// Attend qu'une condition soit remplie pendant une durée maximale définie par TIMEOUT_SECONDS.
        /// </summary>
        /// <param name="condition">Délégué représentant la condition principale à vérifier périodiquement</param>
        /// <param name="cancelCondition">Délégué représentant une condition d'annulation</param>
        /// <returns>
        /// MatchmakingState.OK si la condition principale est remplie avant le délai d'attente,
        /// MatchmakingState.TIMEOUT si le délai d'attente est atteint,
        /// MatchmakingState.RETRY si la condition d'annulation est remplie.
        /// </returns>
        private MatchmakingState WaitForCondition(Func<bool> condition, Func<bool> cancelCondition)
        {
            DateTime startTime = DateTime.Now;
            bool loop = true;
            MatchmakingState state = MatchmakingState.TIMEOUT;
            while (loop)
            {
                if (condition())
                {
                    state = MatchmakingState.OK;
                    loop = false;
                }
                if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS)
                {
                    state = MatchmakingState.TIMEOUT;
                    loop = false;
                }
                if (cancelCondition())
                {
                    state = MatchmakingState.RETRY;
                    loop = false;
                }
                Thread.Sleep(100);
            }
            return state;
        }
    }
}
