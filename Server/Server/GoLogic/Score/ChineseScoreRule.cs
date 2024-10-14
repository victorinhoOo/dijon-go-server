using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoLogic.Score
{
    public class ChineseScoreRule : ScoreRule
    {
        public ChineseScoreRule(GameBoard gameBoard) : base(gameBoard)
        {
        }

        public override (int blackStones, int whiteStones) CalculateScore()
        {
            (int blackStones, int whiteStones) = CountStones();
            (int territoryBlack, int territoryWhite) = FindTerritory();

            return (blackStones + territoryBlack, whiteStones + territoryWhite);
        }

    }
}
