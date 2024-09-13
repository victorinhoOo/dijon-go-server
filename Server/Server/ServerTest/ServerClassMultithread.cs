using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ServerTest
{
    public class ServerClassMultithread
    {
        private Socket handler;

        public ServerClassMultithread(Socket handler)
        {
            this.handler = handler;
        }

        public async void InstanceMethod()
        {
            int idThread = Thread.CurrentThread.ManagedThreadId;
            Console.WriteLine("[Serveur {0:d}] Nouveau client...", idThread);
            bool doLoop = true;
            while (doLoop)
            {
                // Receive message.
                byte[] buffer = new byte[1024];
                int received = await handler.ReceiveAsync(buffer, SocketFlags.None);
                string message = Encoding.UTF8.GetString(buffer, 0, received);
                string eom = "<|EOM|>";
                if (message.IndexOf(eom) > -1 /* is end of message */)
                {
                    Console.WriteLine(
                    $"Socket server received message: \"{message.Replace(eom, "")}\"");
                    string ackMessage = "<|ACK|>";
                    byte[] echoBytes = Encoding.UTF8.GetBytes(ackMessage);
                    await handler.SendAsync(echoBytes, 0);
                    Console.WriteLine(
                    $"Socket server sent acknowledgment: \"{ackMessage}\"");
                    doLoop = false;
                }
                this.handler.Close();
                Console.WriteLine("[Serveur {0:d}] Fin traitement client...", idThread);
            }
        }
    }
}
