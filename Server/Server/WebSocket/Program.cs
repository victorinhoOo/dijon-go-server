using DotNetEnv;
using WebSocket;

Env.Load();
// Récupére les valeurs des variables d'environnement 
string serverIp = Env.GetString("SERVER_IP");
int serverPort = Convert.ToInt32(Env.GetString("SERVER_PORT")); 

// Créé une instance du serveur
WebSocket.Server server = new WebSocket.Server(serverIp, serverPort);
server.Start();