using GoLogic;
using GoLogic.Goban;
using GoLogic.Score;
using GoLogic.Serializer;
using GoLogic.Timer;
using System.Xml.Linq;
using WebSocket.Model.Managers;

namespace WebSocket.Model
{
    /// <summary>
    /// Classe qui représente une partie de jeu de go
    /// </summary>
    public class Game
    {
        private IClient player1;
        private IClient player2;
        private IClient currentTurn;
        private IBoard gameBoard;
        private GameLogic logic;
        private BoardSerializer boardSerializer;
        private ScoreRule score;
        private bool started;
        private int id;
        private GameConfiguration config;
        private TimerManager timerManager;
        private GameManager gameManager;

        /// <summary>
        /// Proprité qui indique si la partie est pleine
        /// </summary>
        public bool IsFull
        {
            get
            {
                return player1 != null && player2 != null;
            }
        }

        /// <summary>
        /// Renvoi si la partie a démarré ou non
        /// </summary>
        public bool Started { get { return this.started; } }

        /// <summary>
        /// Récupérer ou modifier le joueur 1
        /// </summary>
        public IClient Player1 { get => player1; set => player1 = value; }


        /// <summary>
        /// Récupérer ou modifier le joueur 2
        /// </summary>
        public IClient Player2 { get => player2; set => player2 = value; }


        /// <summary>
        /// Récupére le joueur qui doit jouer
        /// </summary>
        public IClient CurrentTurn { get => currentTurn; }

        /// <summary>
        /// Récupère la configuration de la partie
        /// </summary>
        public GameConfiguration Config { get => config; }

        /// <summary>
        /// Récupére l'identifiant de la partie
        /// </summary>
        public int Id { get => id; }


        /// <summary>
        /// Constructeur d'une partie
        /// </summary>
        public Game(GameConfiguration config, GameBoard gameBoard, GameLogic logic, BoardSerializer boardSerializer, ScoreRule scoreRule, GameManager gameManager, TimerManager timerManager)
        {
            this.started = false;
            this.id = Server.CustomGames.Count + 1;

            // Configuration
            this.config = config;

            // Dépendances
            this.gameBoard = gameBoard;
            this.logic = logic;
            this.boardSerializer = boardSerializer;
            this.score = scoreRule;
            this.gameManager = gameManager;
            this.timerManager = timerManager;
        }

        /// <summary>
        /// Démarre la partie
        /// </summary>
        public async void Start()
        {
            this.started = true;
            this.gameManager.InsertGame(this);
            await this.SaveGameState();
        }

        /// <summary>
        /// Ajouter un joueur à la partie
        /// </summary>
        /// <param name="player">Joueur à ajouter</param>
        public void AddPlayer(IClient player)
        {
            if (this.player1 == null)
            {
                this.player1 = player;
                this.currentTurn = player;
            }
            else if (this.player2 == null)
            { 
                this.player2 = player;
            }         
        }


        /// <summary>
        /// Placer une pierre sur le plateau
        /// </summary>
        /// <param name="x">Coordonées en x de la pierre</param>
        /// <param name="y">Coordonnées en y de la pierre</param>
        /// <returns>Temps restant du joueur précédent</returns>
        public async Task<string> PlaceStone(int x, int y)
        {
            this.logic.PlaceStone(x, y);
            this.timerManager.SwitchToNextPlayer();
            await this.SaveGameState();
            string timeLeft = GetPreviousPlayerTime();
            return timeLeft;
        }

        /// <summary>
        /// Enregistre l'état de la partie actuelle en base de données
        /// </summary>
        private async Task SaveGameState()
        {
            await this.gameManager.InsertGameState(this);
        }


        /// <summary>
        /// Récupérer le temps restant du joueur précédent 
        /// </summary>
        /// <returns>temps restant du joueur qui vient de jouer en chaîne de caractères</returns>
        private string GetPreviousPlayerTime()
        {
            string result = null;
            ISystemTimer previousTimer = this.timerManager.GetPreviousTimer();
            TimeSpan previousTimeSpan = previousTimer.TotalTime;
            double previousTimerInMs = previousTimeSpan.TotalMilliseconds;
            double roundedResult = Math.Round(previousTimerInMs);
            result = roundedResult.ToString();
            return result;
        }



        /// <summary>
        /// Changement de tour
        /// </summary>
        public void ChangeTurn()
        {
            this.currentTurn = this.currentTurn == this.player1 ? this.player2 : this.player1;
        }


        /// <summary>
        /// Conversiond de l'état de la partie en chaine de caractère
        /// </summary>
        /// <returns>état de la partie en string</returns>
        public string StringifyGameBoard()
        {
            return boardSerializer.StringifyGoban(logic.CurrentTurn);
        }

        /// <summary>
        /// Récupérer le score de la partie
        /// </summary>
        /// <returns>Score de la partie sous forme de tuple</returns>
        public (float, float) GetScore()
        {
            return score.CalculateScore();
        }
        
        /// <summary>
        /// Récupère les pierres noires et blanches capturées
        /// </summary>
        /// <returns>Tuple d'entier des pierres noires et blanches</returns>
        public (int, int) GetCapturedStone()
        {
            return (gameBoard.CapturedBlackStones, gameBoard.CapturedWhiteStones);
        }


        /// <summary>
        /// Passer son tour
        /// </summary>
        public void SkipTurn()
        {
            logic.SkipTurn();
            ChangeTurn();
        }

        /// <summary>
        /// Test si la partie est terminée. Si oui, déclenche les opérations de BDD en arrière-plan.
        /// </summary>
        /// <returns>True si la partie est terminée, False sinon</returns>
        public Task<bool> TestWinAsync()
        {
            bool result = false;

            if (logic.IsEndGame)
            {
                result = true;

                // Exécuter les tâches BDD en arrière-plan
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await gameManager.TransferMovesToSqlAsync(this);
                        await gameManager.UpdateGameAsync(this);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Erreur lors des opérations de transfert de Redis vers Sqlite : {ex.Message}");
                    }
                });
            }
            return Task.FromResult(result);
        }
    }
}
