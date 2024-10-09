using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket
{
    public interface IWebProtocol
    {
        public void Start();

        public TcpClient AcceptClient(); 
        public byte[] HandShake(string data);

        public byte[] DecryptMessage(byte[] bytes);

        public byte[] BuildMessage(string message);

        public byte[] BuildDeconnection(int code);
    }
}
