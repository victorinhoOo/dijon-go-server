using GoLogic;
using System.Globalization;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using WebSocket.Model;
using WebSocket.Model.DAO;
using WebSocket.Model.DTO;
using WebSocket.Strategy;

namespace WebSocket
{
    /// <summary>
    /// Interprète les messages reçus par le serveur
    /// </summary>
    public class Interpreter
    {
        private Dictionary<string,IStrategy> strategies;

        /// <summary>
        /// Constructeur de la classe Interpreter
        /// </summary>
        public Interpreter()
        {
            this.strategies = new Dictionary<string, IStrategy>()
            {
                { "Stone", new PlaceStoneStrategy() },
                { "Create", new CreateGameStrategy() },
                { "Join", new JoinGameStrategy() },
                { "Matchmaking", new MatchmakingStrategy() },
                { "Skip", new SkipStrategy() }
            };
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
            ValidateAndTruncate(ref data);
            string type = ""; // type de réponse (send ou broadcast)
            string response = "";
            this.strategies[action].Execute(client, data, gameType, ref response, ref type);
            return type + response;

        }

        /// <summary>
        /// Valide et tronque les valeurs selon les limites imposées 
        /// </summary>
        /// <param name="data">Tableau des données à valider</param>
        private void ValidateAndTruncate(ref string[] data)
        {
            // Vérifier et tronquer data[6] les valeurs en dessous de 0 ne sont pas accepté
            if (data[6].Length > 3)
            {
                float komi = float.Parse(data[6], CultureInfo.InvariantCulture.NumberFormat);
                if (komi > 999)
                    komi = 999;
                else if( komi < 0)
                    komi = 0;
                data[6] = komi.ToString();
            }
            
            // Vérifier et tronquer data[7] à 15 caractères
            if (data[7].Length > 15)
                data[7] = data[7].Substring(0, 15);
            

            // Vérifier et tronquer data[8] à une longueur de 1 caractère, avec une limite de "9"
            if (data[8].Length > 1)
            {
                float handicap = float.Parse(data[8], CultureInfo.InvariantCulture.NumberFormat);
                if (handicap > 9)
                    handicap = 9;
                else if (handicap < 0)
                    handicap = 0;
                data[8] = handicap.ToString();
            }
        }

    }
}
