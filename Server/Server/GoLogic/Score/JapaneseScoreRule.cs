using GoLogic.Goban;

namespace GoLogic.Score
{
    /// <summary>
    /// Classe pour gérer le décompte des points
    /// Selon les règles Japonaise
    /// </summary>
    public class JapaneseScoreRule : ScoreRule
    {
        public JapaneseScoreRule(IBoard gameBoard, float komi = 6.5f) : base(gameBoard, komi) { }

        /// <summary>
        /// Règles de décompte des points Japonaise
        /// </summary>
        /// <returns>Tuple d'entier correspondant aux scores noir et blanc</returns>
        public override (float blackStones, float whiteStones) CalculateScore()
        {
            RemoveDeadStones();
            (int blackStones, int whiteStones) = this.gameBoard.CountStones();
            (int territoryBlack, int territoryWhite) = (0, 0);
            if (blackStones + whiteStones > 1)
            {
                (territoryBlack, territoryWhite) = FindTerritory();
            }
            return (territoryBlack + this.gameBoard.CapturedWhiteStones, territoryWhite + this.gameBoard.CapturedBlackStones + this.komi);
        }
    }
}
