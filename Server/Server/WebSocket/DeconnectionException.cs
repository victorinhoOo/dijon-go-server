using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket
{
    public class DeconnectionException  : Exception
    {
        private int code; 

        public int Code { get { return code; } }
        public DeconnectionException(int code):base($"Client deconnected with status {code}")
        {
            this.code = code;
        }
    }
}
