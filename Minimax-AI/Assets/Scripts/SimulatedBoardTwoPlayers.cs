using BoardGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BoardGame
{
    public class SimulatedBoardTwoPlayers : MonoBehaviour
    {
        public Text _boardText;
        public Vector2Int _size;

        [Min(1)]
        [SerializeField]
        private int _simulationDepth = 3;
        
        GameExecutor _gameExecutor;
        IEvaluator _boardEvaluator;

        public bool _useAlphaBeta = false;

        private Board _board;
        private Player _p1;
        private Player _p2;
        private int _turn = 0;


        // Start is called before the first frame update
        void Start()
        {
            InitBoard();
            _gameExecutor = new GameExecutor(_board, DisplayBoard);
            _boardEvaluator = new ColumnGameEvaluator(5);

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
            _p1 = new Player(0);
            _p2 = new Player(1);
            _board = new Board(_size);

            for (int i = 0; i < _size.x; i++)
            {
                for (int j = 0; j < _size.y; j++)
                {
                    _board.tiles[i, j].pointMultiplier = (int)Random.Range(2, 6);
                }
            }
        }

        IEnumerator SimulationCoroutine()
        {
            while(_board.EndState()==false)
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
            if (_turn % 2 == 0)
            {
                //P1 move
                player1 = _p1;
                player2 = _p2;
            }
            else
            {
                //P2 move
                player1 = _p2;
                player2 = _p1;
            }

            List<ICommand> moves = _board.PossibleMoves(player1);

            int bestScore = int.MinValue;
            ICommand bestMove = null;
            foreach (ICommand move in moves)
            {
                Board afterMove = _board.Copy();
                _gameExecutor.ExecuteCommand(afterMove, move);
                int score = 0;
                if (_useAlphaBeta)
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
                _gameExecutor.ExecuteCommand(bestMove);
            }
            else
            {
                Debug.Log("No possible moves. Game has ended.");
            }

            _turn++;
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
            if (board.EndState() == true || depth == _simulationDepth)
            {
                return _boardEvaluator.Evaluate(board, playerFirst) - _boardEvaluator.Evaluate(board, playerSecond);
            }

            if (depth % 2 == 0)
            {
                int bestScore = int.MinValue;
                List<ICommand> moves = board.PossibleMoves(playerFirst);

                foreach (ICommand move in moves)
                {
                    Board afterMove = board.Copy();
                    _gameExecutor.ExecuteCommand(afterMove, move);
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
                    _gameExecutor.ExecuteCommand(afterMove, move);
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
            if (board.EndState() == true || depth == _simulationDepth)
            {
                return _boardEvaluator.Evaluate(board, playerFirst) - _boardEvaluator.Evaluate(board, playerSecond);
            }

            if (depth % 2 == 0)
            {
                int bestScore = int.MinValue;
                List<ICommand> moves = board.PossibleMoves(playerFirst);

                foreach (ICommand move in moves)
                {
                    Board afterMove = board.Copy();
                    _gameExecutor.ExecuteCommand(afterMove, move);
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
                    _gameExecutor.ExecuteCommand(afterMove, move);
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
            for (int i = 0; i < _size.y; i++)
            {
                for (int j = 0; j < _size.x; j++)
                {
                    if (_board.tiles[j, i].Occupied() == false)
                    {
                        board += " " + (int)(_board.tiles[j, i].pointMultiplier) + " ";
                    }
                    else
                    {
                        if (_board.tiles[j, i].pawn.owner == _p1)
                        {
                            board += " <color=red>" + (int)(_board.tiles[j, i].pointMultiplier) + "</color> ";
                        }
                        else
                        {
                            board += " <color=blue>" + (int)(_board.tiles[j, i].pointMultiplier) + "</color> ";
                        }
                    }
                }
                board += "\n";
            }

            _boardText.text = board;
            _boardText.text += $"\n\n  <color=red>P1: {_boardEvaluator.Evaluate(_board, _p1)}</color>";
            _boardText.text += $"\n\n  <color=blue> P2: {_boardEvaluator.Evaluate(_board, _p2)}</color>";
        }
    }
}

