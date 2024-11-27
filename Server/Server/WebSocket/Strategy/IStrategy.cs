namespace WebSocket.Strategy
{
    public interface IStrategy
    {
        public void execute(Client player, string[] data, string gameType, ref string response, ref string type);
    }
}
