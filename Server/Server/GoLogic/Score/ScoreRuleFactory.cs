using GoLogic.Goban;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoLogic.Score
{
    /// <summary>
    /// Gère la création des règles de score
    /// </summary>
    public static class ScoreRuleFactory
    {
        /// <summary>
        /// Crée une instance de règle de score en fonction du type de règle spécifié.
        /// </summary>
        /// <param name="ruleType">Le type de règle ("c" pour Chinoise, "j" pour Japonaise, etc.)</param>
        /// <param name="gameBoard">Le plateau de jeu</param>
        /// <param name="komi">Le komi à utiliser (par défaut 6.5)</param>
        /// <returns>Une instance de ScoreRule correspondante</returns>
        /// <exception cref="ArgumentException">Levée si le type de règles donnée est inconnu</exception>
        public static ScoreRule Create(string ruleType, IBoard gameBoard, float komi = 6.5f)
        {
            return ruleType switch
            {
                "c" => new ChineseScoreRule(gameBoard, komi),
                "j" => new JapaneseScoreRule(gameBoard, komi),
                _ => throw new ArgumentException($"Type de règle inconnu : {ruleType}")
            };
        }
    }
}
