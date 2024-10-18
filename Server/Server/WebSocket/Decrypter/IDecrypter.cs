using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket.Decrypter
{
    public interface IDecrypter
    {
        public byte[] Decrypt(byte[] bytes);
    }
}
