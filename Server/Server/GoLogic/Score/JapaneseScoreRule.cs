namespace GoLogic.Score
{
    /// <summary>
    /// Classe pour gérer le décompte des points
    /// Selon les règles Japonaise
    /// </summary>
    public class JapaneseScoreRule : ScoreRule
    {
        public JapaneseScoreRule(GameBoard gameBoard) : base(gameBoard) { }

        /// <summary>
        /// Règles de décompte des points Japonaise
        /// </summary>
        /// <returns>Tuple d'entier correspondant aux scores noir et blanc</returns>
        public override (int blackStones, int whiteStones) CalculateScore()
        {
            (int blackStones, int whiteStones) = CountStones();
            (int territoryBlack, int territoryWhite) = (0, 0);
            if (blackStones + whiteStones > 0)
            {
                (territoryBlack, territoryWhite) = FindTerritory();
            }
            return (territoryBlack + GameBoard.CapturedWhiteStones, territoryWhite + GameBoard.CapturedBlackStones);
        }
    }
}
