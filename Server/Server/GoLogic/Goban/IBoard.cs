using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoLogic.Goban
{
    /// <summary>
    /// Represents the game board and its operations
    /// </summary>
    public interface IBoard
    {
        /// <summary>
        /// Récupère la taille du plateau
        /// </summary>
        int Size { get; }

        /// <summary>
        /// Tour actuel, Noir ou Blanc
        /// </summary>
        public StoneColor CurrentTurn { get; }

        /// <summary>
        /// Pierre noire qui ont été capturée
        /// </summary>
        public int CapturedBlackStones { get; }

        /// <summary>
        /// Pierre blanche qui ont été capturée
        /// </summary>
        public int CapturedWhiteStones { get; }

        /// <summary>
        /// Change le tour des joueur
        /// </summary>
        public void NextTurn();

        /// <summary>
        /// Ajoute au joueur de la couleur spécifié
        /// le nombre de pierres capturer
        /// </summary>
        /// <param name="color">Couleur des pierres capturé</param>
        /// <param name="amount">quantité de pierres capturé</param>
        public void AddCapturedStone(StoneColor color, int amount);

        /// <summary>
        /// Gets the stone at the specified coordinates
        /// </summary>
        /// <param name="x">coordonnées X</param>
        /// <param name="y">coordonnées X</param>
        /// <returns>La pierre aux coordoonées spécifié</returns>
        Stone GetStone(int x, int y);

        /// <summary>
        /// Change la couleur de la pierre spécifié
        /// </summary>
        /// <param name="stone">La pierre à changer la couleur</param>
        /// <param name="color">Couleur de la pierre à placer</param>
        /// <returns>A new board state with the stone placed</returns>
        void PlaceStone(Stone stone, StoneColor color);

        /// <summary>
        /// Vérifie si les coordonnées sont correct
        /// </summary>
        /// <param name="x">coordonnées X</param>
        /// <param name="y">coordonnées Y</param>
        /// <returns>True si les coordonnées sont correct, false sinon</returns>
        bool IsValidCoordinate(int x, int y);

        /// <summary>
        /// Créer une copie complete de IBoard
        /// </summary>
        /// <returns>Une nouvelle instance de IBoard avec les même valeurs</returns>
        IBoard Clone();

        /// <summary>
        /// Récupères les pierres voisines de celle spécifiée
        /// </summary>
        /// <param name="stone">La pierre dont on cherche les voisines</param>
        /// <returns>Liste des pierres voisines</returns>
        public List<Stone> GetNeighbors(Stone stone);

        /// <summary>
        /// Vérifie si l'état actuel du plateau correspond à l'état précédent (règle de Ko).
        /// Pour éviter que le jeu tourne en boucle
        /// </summary>
        /// <returns>Vraie si le coup ne respecte pas la règle, faux sinon</returns>
        public bool IsKoViolation();

        /// <summary>
        /// Compte les nombres de pierres de chaque couleur sur le plateau
        /// </summary>
        /// <returns>Un tuple d'entier correspondant aux pierres noires et blanches</returns>
        public (int blackStones, int whiteStones) CountStones();
    }
}
