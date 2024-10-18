using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket.Exceptions
{
    /// <summary>
    /// Exception qui sera levée lorsqu'un client client se déconnecte du serveur
    /// </summary>
    public class DisconnectionException : Exception
    {
        private int code;

        /// <summary>
        /// Récupère le code de la déconnexion du client
        /// </summary>
        public int Code { get { return code; } }

        public DisconnectionException(int code) : base($"Client deconnected with status {code}")
        {
            this.code = code;
        }
    }
}
