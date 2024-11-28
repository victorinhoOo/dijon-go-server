using GoLogic.Goban;

namespace GoLogic.Score
{
    /// <summary>
    /// Classe pour gérer le décompte des points
    /// Selon les règles Chinoise
    /// </summary>
    public class ChineseScoreRule : ScoreRule
    {
        public ChineseScoreRule(GameBoard gameBoard) : base(gameBoard) { }

        /// <summary>
        /// Règles de décompte des points Chinoise
        /// </summary>
        /// <returns>Tuple d'entier correspondant aux scores noir et blanc</returns>
        public override (int blackStones, int whiteStones) CalculateScore()
        {
            (int blackStones, int whiteStones) = CountStones();
            (int territoryBlack, int territoryWhite) = FindTerritory();

            return (blackStones + territoryBlack, whiteStones + territoryWhite);
        }

    }
}
