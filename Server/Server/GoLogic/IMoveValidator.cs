using GoLogic.Goban;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoLogic
{
    /// <summary>
    /// Contient les vérification nécessaire pour jouer un coup
    /// </summary>
    public interface IMoveValidator
    {
        /// <summary>
        /// Vérifie si le coup est valide selon les règles du GO
        /// </summary>
        /// <param name="stone">La pierre placée sur le plateau</param>
        /// <returns>True si le coup est valide, False sinon</returns>
        bool IsValidMove(Stone stone);
    }
}
