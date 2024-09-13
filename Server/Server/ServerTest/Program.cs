using ServerTest;
using System.Net;
using System.Net.Sockets;

IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync("localhost");
IPAddress ipAddress = ipHostInfo.AddressList[0];
IPEndPoint ipEndPoint = new(ipAddress, 11000);


using Socket listener = new(
ipEndPoint.AddressFamily,
SocketType.Stream,
ProtocolType.Tcp);

listener.Bind(ipEndPoint);
listener.Listen(100);

int cpt = 0;
while (cpt < 5)
{
    cpt++;
    Socket handler = await listener.AcceptAsync();
    ServerClassMultithread serverObject = new ServerClassMultithread(handler);
    Thread InstanceCaller = new Thread(new
    ThreadStart(serverObject.InstanceMethod));
    InstanceCaller.Start(); // je démarre le thread
}
listener.Close();