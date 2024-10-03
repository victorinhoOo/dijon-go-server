using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerTest
{
    public class Server2
    {
        private Socket listener;
        private bool isRunning;

        public Server2()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
            IPAddress ipAdress = ipHostInfo.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAdress, 7000);

            this.listener = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            this.listener.Bind(ipEndPoint);
        }

        public void Start()
        {
            this.listener.Listen(100);
            this.isRunning = true;

            while (isRunning)
            {
                try
                {
                    Thread thread = new Thread(() =>
                    {
                        Socket handler = this.listener.Accept();
                        byte[] buffer = new byte[1024];
                        int received = handler.Receive(buffer, SocketFlags.None);
                        string message = Encoding.UTF8.GetString(buffer, 0, received);

                        string eom = "\r\n";
                        if (message.IndexOf(eom) > -1)
                        {
                            Console.WriteLine($"Socket server received message: \"{message.Replace(eom, "")}\"");
                            string ack = "HTTP/1.0 200 OK\r\n" + "Content-Type: text/html\r\n" + "\r\n";
                            byte[] response = Encoding.UTF8.GetBytes(ack);
                            handler.Send(response);
                            Console.WriteLine($"Socket server sent acknowledgment: \"{ack}\"");
                        }

                    });
                }
                catch (Exception e)
                {

                }
            }
        }
    }
}
