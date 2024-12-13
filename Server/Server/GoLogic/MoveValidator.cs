using GoLogic.Goban;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoLogic
{
    /// <inheritdoc/>
    public class MoveValidator : IMoveValidator
    {
        private IBoard goban;
        
        /// <summary>
        /// Constructeur de MoveValidator
        /// </summary>
        /// <param name="board">Le GameBoard à valider</param>
        /// <param name="captureManager">Le captureManager pour la validation</param>
        public MoveValidator(IBoard board) 
        {
            this.goban = board;
        }

        /// <summary>
        /// Vérifie si le coup est valide selon les règles du GO
        /// </summary>
        /// <param name="stone">La pierre placée sur le plateau</param>
        /// <returns>True si le coup est valide, False sinon</returns>
        public bool IsValidMove(Stone stone)
        {
            IBoard boardCopy = this.goban.Clone();
            CaptureManager captureManagerCopy = new CaptureManager(boardCopy);
            Stone stoneCopy = boardCopy.GetStone(stone.X, stone.Y);

            bool result = true;

            // Vérifie que le coup est dans les limites du plateau et que l'emplacement est vide
            if (stoneCopy.Color != StoneColor.Empty || !this.goban.IsValidCoordinate(stoneCopy.X, stoneCopy.Y))
                result = false;

            else
            {
                // Place la pierre dans une copie pour vérifier les libertés et les captures
                stoneCopy.ChangeColor(boardCopy.CurrentTurn);

                // 1. Vérifie si la pierre a des libertés et capture des pierres adverses
                // Vérifie si le coup entraînerait un "suicide" (pas de libertés)
                // On l'autorise seulement s'il capture des pierres adverses
                if (!captureManagerCopy.HasLiberties(stoneCopy) && !captureManagerCopy.CheckCapture(stoneCopy))
                {
                    stoneCopy.ChangeColor(StoneColor.Empty); // Annule le coup
                    result = false; // Coup invalidé (car suicide)
                }

                captureManagerCopy.CapturesOpponent(stoneCopy); // On execute la capture pour vérifier ko
                // 2. Vérifie la règle de Ko (empêche de répéter l'état précédent du plateau)
                if(boardCopy.IsKoViolation())
                {
                    result = false;
                }
            }

            return result;
        }
    }
}
