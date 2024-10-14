using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoLogic.Score
{
    public class JapaneseScoreRule : ScoreRule
    {
        public JapaneseScoreRule(GameBoard gameBoard) : base(gameBoard)
        {
        }

        public override (int blackStones, int whiteStones) CalculateScore()
        {
            (int territoryBlack, int territoryWhite) = FindTerritory();
            return (territoryBlack + GameBoard.CapturedWhiteStones, territoryWhite + GameBoard.CapturedBlackStones);
        }
    }
}
