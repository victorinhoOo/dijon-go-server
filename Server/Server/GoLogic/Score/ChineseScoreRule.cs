using GoLogic.Goban;

namespace GoLogic.Score
{
    /// <summary>
    /// Classe pour gérer le décompte des points
    /// Selon les règles Chinoise
    /// </summary>
    public class ChineseScoreRule : ScoreRule
    {
        public ChineseScoreRule(IBoard gameBoard, float komi = 6.5f) : base(gameBoard, komi) { }

        /// <summary>
        /// Règles de décompte des points Chinoise
        /// </summary>
        /// <returns>Tuple d'entier correspondant aux scores noir et blanc</returns>
        public override (float blackStones, float whiteStones) CalculateScore()
        {
            RemoveDeadStones();
            (int blackStones, int whiteStones) = this.gameBoard.CountStones();
            (int territoryBlack, int territoryWhite) = FindTerritory();

            return (blackStones + territoryBlack, whiteStones + territoryWhite + komi);
        }

    }
}
    