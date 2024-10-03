using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerTest
{
    public class Client
    {
        private TcpClient tcpClient;
        private StreamReader sr;

        public Client(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            this.sr = new StreamReader(tcpClient.GetStream());
        }
    }
}
