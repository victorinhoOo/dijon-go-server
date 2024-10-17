using GoLogic;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using WebSocket.Model.DAO;

namespace WebSocket
{
    public class Interpreter
    {
        private GameDAO gameDAO;

        public Interpreter()
        {
            this.gameDAO = new GameDAO();
        }

        public string Interpret(string message, Client client)
        {

            int idGame = Convert.ToInt32(message.Split("/")[0]);
            message = message.Split("/")[1];
            string action = message.Split(":")[0];
            string type = "";
            string response = "";
            switch (action)
            {
                case "Stone": this.PlaceStone(client, idGame, message.Split(':')[1], ref response, ref type);break;
                case "Create": this.CreateGame(client, ref response, ref type); break;
                case "Join": this.JoinGame(client, idGame, ref response, ref type);break;

            }
            return type+response;

        }


        private void PlaceStone(Client player, int idGame, string coordinates, ref string response, ref string type)
        {
            if(idGame != 0)
            {
                Game game = Server.Games[idGame];
                if(game.CurrentTurn == player)
                {
                    try
                    {
                        int x = Convert.ToInt32(coordinates.Split("-")[0]);
                        int y = Convert.ToInt32(coordinates.Split("-")[1]);

                        game.PlaceStone(x, y);

                        game.ChangeTurn();
                        response = $"{idGame}/{game.StringifyGameBoard()}";
                        type = "Broadcast_";
                    }
                    catch (Exception e)
                    {
                        response = $"{idGame}/Error:{e.Message}";
                        type = "Send_";
                    }
                }
                else
                {
                    response = $"{idGame}/Not your turn";
                    type = "Send_";
                }
            }
        }

        private void CreateGame(Client client, ref string response, ref string type)
        {
            int id  = Server.Games.Count + 1;
            Game newGame = new Game(client);
            Server.Games[id] = newGame;
            
            this.gameDAO.InsertGame(newGame);
            Server.Games[id].Player1 = client;
            response = $"{id}/";
            type = "Send_";
        }

        private void JoinGame(Client client, int idGame, ref string reponse, ref string type)
        {
            // exception
            Server.Games[idGame].AddPlayer(client);
            this.gameDAO.DeleteGame(idGame);
            reponse = $"{idGame}/";
            type = "Send_";
        }

    }
}
