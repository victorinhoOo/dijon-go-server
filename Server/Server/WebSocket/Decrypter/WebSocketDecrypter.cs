using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocket.Exceptions;

namespace WebSocket.Decrypter
{
    public class WebSocketDecrypter : IDecrypter
    {
        /// <inheritdoc/>
        public byte[] Decrypt(byte[] bytes)
        {
            byte[] decoded = bytes;
            if ((bytes[0] & 0b10000000) != 0) // Vérifie que le message n'est pas fragmenté
            {
                bool mask = (bytes[1] & 0b10000000) != 0; // Doit être vrai, cela détermine si le message est masqué ce qui est toujours le cas
                // lorsque le message provient du client

                ulong offset = 2; // Entier non signé qui détermine le nombre d'octets précédents les 4 octets de masque
                ulong messageLength = bytes[1] & (ulong)0b01111111; // Entier non signé qui détermine la taille du message envoyé

                if (messageLength == 126) // Cela signifie que la taille du message reçu est déterminée par les deux prochains octets
                {
                    messageLength = BitConverter.ToUInt16(new byte[] { bytes[3], bytes[2] }, 0); // Inversion des octets représentant la taille du message 
                    // Car BitConverter utilise la représentation Big Endian alors que nous utilisons la représentation Little Endian 
                    offset = 4; // Le décalage est donc de 4 octets 
                }

                else if (messageLength == 127) // Cela signifie que la taille du message reçu est déterminée par les 8 prochains octets
                {
                    messageLength = BitConverter.ToUInt64(new byte[] { bytes[9], bytes[8], bytes[7], bytes[6], bytes[5], bytes[4], bytes[3], bytes[2] }, 0); //Inversion de octets
                    offset = 10; // Le décalage est donc de 10 octets 
                }

                if (messageLength == 0) // La taille du message reçu est à 0
                {
                    Console.WriteLine("The message received is empty");
                }

                else if (mask) // Si le masque est correctement paramétré
                {
                    decoded = new byte[messageLength];
                    byte[] masks = new byte[4] { bytes[offset], bytes[offset + 1], bytes[offset + 2], bytes[offset + 3] }; // Ajout des 4 octets du masque
                    offset += 4; // Ajout de 4 au décalage pour atteindre les octets du message

                    for (ulong i = 0; i < messageLength; ++i)
                    {
                        decoded[i] = (byte)(bytes[offset + i] ^ masks[i % 4]); // formule de déchiffrage du message octet par octet 
                    }
                }
                else // Erreur de masque
                {
                    Console.WriteLine("Mask error");
                }

                int opcode = bytes[0] & 0b00001111;
                if (opcode == 8) // Le message envoyé est un message de déconnexion 
                {
                    ushort code = BitConverter.ToUInt16(new byte[] { decoded[1], decoded[0] }, 0); // Calcul du code de déconnexion
                    throw new DisconnectionException(code);
                }

            }
            return decoded;
        }
    }
}
