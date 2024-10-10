using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket
{
    /// <summary>
    /// Exception qui sera levée lorsqu'un client client se déconnecte du serveur
    /// </summary>
    public class DeconnectionException  : Exception
    {
        private int code; 

        /// <summary>
        /// Récupère le code de la déconnexion du client
        /// </summary>
        public int Code { get { return code; } } 

        public DeconnectionException(int code):base($"Client deconnected with status {code}")
        {
            this.code = code;
        }
    }
}
