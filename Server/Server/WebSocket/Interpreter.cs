using GoLogic;
using System.Globalization;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using WebSocket.Model;
using WebSocket.Model.DAO;
using WebSocket.Model.DTO;
using WebSocket.Strategy;
using WebSocket.Strategy.Enumerations;

namespace WebSocket
{

    /// <summary>
    /// Interprète les messages reçus par le serveur
    /// </summary>
    public class Interpreter
    {
        private const int ACTION_INDEX = 1;
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
                { "Skip", new SkipStrategy() },
                {"Cancel", new CancelStrategy() },
                { "Chat", new ChatStrategy() }
            };
        }

        /// <summary>
        /// Réagit au message reçu par le serveur
        /// </summary>
        /// <param name="message">Message reçu</param>
        /// <param name="client">client qui expédie le message</param>
        /// <returns>la réponse du serveur au client</returns>
        public string Interpret(string message, IClient client, GameType gameType)
        {
            string[] data = message.Split("-");
            string action = data[ACTION_INDEX]; // action à effectuer
            string type = ""; // type de réponse (send ou broadcast)
            string response = "";
            this.strategies[action].Execute(client, data, gameType, ref response, ref type);
            return type + response;

        }
    }
}
