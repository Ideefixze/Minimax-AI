using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BoardGame
{
    public class SimulatedBoardTwoPlayers : MonoBehaviour
    {
        public Text BoardText;
        public Vector2Int Size;

        [Min(1)]
        [SerializeField]
        private int simulationDepth = 3;
        
        GameExecutor gameExecutor;
        IEvaluator boardEvaluator;

        public bool UseAlphaBeta = false;

        private Board board;
        private Player p1;
        private Player p2;
        private int turn = 0;


        // Start is called before the first frame update
        void Start()
        {
            InitBoard();
            gameExecutor = new GameExecutor(board, DisplayBoard);
            boardEvaluator = new ColumnGameEvaluator(5);

            DisplayBoard();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Simulate();
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                StopAllCoroutines();
                StartCoroutine("SimulationCoroutine");
            }
        }

        public void InitBoard()
        {
            p1 = new Player(0);
            p2 = new Player(1);
            board = new Board(Size);

            for (int i = 0; i < Size.x; i++)
            {
                for (int j = 0; j < Size.y; j++)
                {
                    board.Tiles[i, j].PointMultiplier = (int)Random.Range(2, 6);
                }
            }
        }

        IEnumerator SimulationCoroutine()
        {
            while(board.EndState()==false)
            {
                Simulate();
                yield return new WaitForEndOfFrame();
            }
            yield return null;
        }

        private void Simulate()
        {

            Player player1; //Player doing this move
            Player player2; //Player next in turn
            if (turn % 2 == 0)
            {
                //P1 move
                player1 = p1;
                player2 = p2;
            }
            else
            {
                //P2 move
                player1 = p2;
                player2 = p1;
            }

            List<ICommand> moves = board.PossibleMoves(player1);

            int bestScore = int.MinValue;
            ICommand bestMove = null;
            foreach (ICommand move in moves)
            {
                Board afterMove = board.Copy();
                gameExecutor.ExecuteCommand(afterMove, move);
                int score = 0;
                if (UseAlphaBeta)
                {
                    score = AlphaBeta(afterMove, 1, player1, player2, int.MinValue, int.MaxValue);
                }
                else
                {
                    score = Minimax(afterMove, 1, player1, player2);
                }
                 
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }
            }

            if (bestMove != null)
            {
                gameExecutor.ExecuteCommand(bestMove);
            }
            else
            {
                Debug.Log("No possible moves. Game has ended.");
            }

            turn++;
        }

        /// <summary>
        /// Minimax without alpha-beta pruning (slower).
        /// </summary>
        /// <param name="board">Board of this game</param>
        /// <param name="depth">Depth of move tree</param>
        /// <param name="playerFirst">Maximizing player</param>
        /// <param name="playerSecond">Minimizing</param>
        /// <returns></returns>
        public int Minimax(Board board, int depth, Player playerFirst, Player playerSecond)
        {
            if (board.EndState() == true || depth == simulationDepth)
            {
                return boardEvaluator.Evaluate(board, playerFirst) - boardEvaluator.Evaluate(board, playerSecond);
            }

            if (depth % 2 == 0)
            {
                int bestScore = int.MinValue;
                List<ICommand> moves = board.PossibleMoves(playerFirst);

                foreach (ICommand move in moves)
                {
                    Board afterMove = board.Copy();
                    gameExecutor.ExecuteCommand(afterMove, move);
                    int score = Minimax(afterMove, depth + 1, playerFirst, playerSecond);
                    bestScore = Mathf.Max(bestScore, score);
                }
                return bestScore;
            }
            else
            {
                int bestScore = int.MaxValue;
                List<ICommand> moves = board.PossibleMoves(playerSecond);

                foreach (ICommand move in moves)
                {
                    Board afterMove = board.Copy();
                    gameExecutor.ExecuteCommand(afterMove, move);
                    int score = Minimax(afterMove, depth + 1, playerFirst, playerSecond);
                    bestScore = Mathf.Min(bestScore, score);
                }
                return bestScore;
            }
        }

        /// <summary>
        /// Minimax with alpha-beta pruning (faster).
        /// </summary>
        /// <param name="board">Board of this game</param>
        /// <param name="depth">Depth of move tree</param>
        /// <param name="playerFirst">Maximizing player</param>
        /// <param name="playerSecond">Minimizing</param>
        /// <param name="alpha">Alpha parameter</param>
        /// <param name="beta">Beta parameter</param>
        /// <returns></returns>
        public int AlphaBeta(Board board, int depth, Player playerFirst, Player playerSecond, int alpha, int beta)
        {
            if (board.EndState() == true || depth == simulationDepth)
            {
                return boardEvaluator.Evaluate(board, playerFirst) - boardEvaluator.Evaluate(board, playerSecond);
            }

            if (depth % 2 == 0)
            {
                int bestScore = int.MinValue;
                List<ICommand> moves = board.PossibleMoves(playerFirst);

                foreach (ICommand move in moves)
                {
                    Board afterMove = board.Copy();
                    gameExecutor.ExecuteCommand(afterMove, move);
                    int score = AlphaBeta(afterMove, depth + 1, playerFirst, playerSecond, alpha, beta);
                    bestScore = Mathf.Max(bestScore, score);
                    alpha = Mathf.Max(alpha, bestScore);
                    if (alpha >= beta)
                        break;
                }
                return bestScore;
            }
            else
            {
                int bestScore = int.MaxValue;
                List<ICommand> moves = board.PossibleMoves(playerSecond);

                foreach (ICommand move in moves)
                {
                    Board afterMove = board.Copy();
                    gameExecutor.ExecuteCommand(afterMove, move);
                    int score = AlphaBeta(afterMove, depth + 1, playerFirst, playerSecond, alpha, beta);
                    bestScore = Mathf.Min(bestScore, score);
                    beta = Mathf.Min(beta, bestScore);
                    if (beta <= alpha)
                        break;
                }
                return bestScore;
            }
        }

        public void DisplayBoard()
        {
            string board = "";
            for (int i = 0; i < Size.y; i++)
            {
                for (int j = 0; j < Size.x; j++)
                {
                    if (this.board.Tiles[j, i].Occupied() == false)
                    {
                        board += " " + (int)(this.board.Tiles[j, i].PointMultiplier) + " ";
                    }
                    else
                    {
                        if (this.board.Tiles[j, i].Pawn.Owner == p1)
                        {
                            board += " <color=red>" + (int)(this.board.Tiles[j, i].PointMultiplier) + "</color> ";
                        }
                        else
                        {
                            board += " <color=blue>" + (int)(this.board.Tiles[j, i].PointMultiplier) + "</color> ";
                        }
                    }
                }
                board += "\n";
            }

            BoardText.text = board;
            BoardText.text += $"\n\n  <color=red>P1: {boardEvaluator.Evaluate(this.board, p1)}</color>";
            BoardText.text += $"\n\n  <color=blue> P2: {boardEvaluator.Evaluate(this.board, p2)}</color>";
        }
    }
}

