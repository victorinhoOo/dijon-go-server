using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebSocket
{
    /// <summary>
    /// Classe qui permet de créer des connexions websocket
    /// </summary>
    public class WebSocket : IWebProtocol
    {
        private TcpListener listener;
        private IDecrypter decrypter;

        public WebSocket(string ipAdress, int port)
        {
            this.listener = new TcpListener(IPAddress.Parse(ipAdress), port);
            this.decrypter = new WebSocketDecrypter();
        }

        /// <inheritdoc/>
        public void Start()
        {
            this.listener.Start();
        }

        /// <inheritdoc/>
        public TcpClient AcceptClient()
        {
            return this.listener.AcceptTcpClient();
        }

        /// <inheritdoc/>
        public byte[] BuildMessage(string message)
        {
            byte[] charsBytes = Encoding.UTF8.GetBytes((message.ToCharArray())); // transformation du message en tableau d'octets
            int messageLength = message.Length;
            int lengthIndicator = 0;
            byte[] length = new byte[] { };
            switch (messageLength)
            {
                case <= 125:lengthIndicator = messageLength;break;
                case <= 65535:
                    {
                        lengthIndicator = 126;
                        length = BitConverter.GetBytes(Convert.ToInt16(messageLength));
                        Array.Reverse(length);
                        break;
                    }
            }
            List<byte> messageBytes = new List<byte>() { 129, Convert.ToByte(lengthIndicator) }; // préparation de la trame
            if (lengthIndicator == 126) // si la longueur du message est supérieure à 125 octets
            {
                foreach(byte b in length)
                {
                   messageBytes.Add(b);
                }
            }
            foreach (byte b in charsBytes)
            {
                messageBytes.Add(b); // Transformation de chaque charactère du message en octet et ajout à la trame
            }
            return messageBytes.ToArray();
        }

        /// <inheritdoc/>
        public byte[] DecryptMessage(byte[] bytes)
        {
           return this.decrypter.Decrypt(bytes);
        }

        /// <inheritdoc/>
        public byte[] BuildHandShake(string data)
        {
            string secretKey = "";
            foreach (string line in data.Split("\r\n"))
            {
                if (line.StartsWith("Sec-WebSocket-Key"))
                {
                    secretKey = line.Split(": ")[1].Trim();
                }
            }

            string concatenated = secretKey + "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
            SHA1 sha1 = SHA1.Create();
            byte[] hashBytes = sha1.ComputeHash(Encoding.ASCII.GetBytes(concatenated));

            string hash = Convert.ToBase64String(hashBytes);

            string response = "HTTP/1.1 101 Switching Protocols\r\n" + "Upgrade: websocket\r\n" + "Connection: Upgrade\r\n" + $"Sec-WebSocket-Accept: {hash}\r\n\r\n";

            return Encoding.UTF8.GetBytes(response);
        }

        /// <inheritdoc/>
        public byte[] BuildDeconnection(int code)
        {
            byte[] codeBytes = BitConverter.GetBytes(Convert.ToInt16(code));
            Array.Reverse(codeBytes);
            List<byte> deconnectionBytes = new List<byte>() { 136, Convert.ToByte(codeBytes.Length) };
            foreach(byte b in codeBytes)
            {
                deconnectionBytes.Add(b);
            }
            return deconnectionBytes.ToArray();
            
        }

        
    }
}
