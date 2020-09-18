using BoardGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    public class SimulatedBoardTwoPlayers : BoardView
    {
        [Min(1)]
        [SerializeField]
        private int _simulationDepth = 3;
        int turn = 0;
        GameExecutor gameExecutor;
        IEvaluator boardEvaluator;

        // Start is called before the first frame update
        void Start()
        {
            base.Start();
            gameExecutor = new GameExecutor(_board, DisplayBoard);
            boardEvaluator = new ColumnGameEvaluator(3);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Simulate();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                gameExecutor.ExecuteCommand(new PlacePawnCommand(new Pawn(new Vector2Int(0, 0), 1, _p1)));
            }
        }

        private void Simulate()
        {

            Player player1; //Player doing this move
            Player player2; //Player next in turn
            if (turn % 2 == 0)
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
                gameExecutor.ExecuteCommand(afterMove, move);
                int score = Minimax(afterMove, 1, player1, player2);
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }
            }

            if (bestMove != null)
            {
                Debug.Log("Best score = " + bestScore);
                gameExecutor.ExecuteCommand(bestMove);
            }
            else
            {
                Debug.Log("No possible moves. Game has ended.");
            }

            turn++;
        }

        public int Minimax(Board board, int depth, Player playerFirst, Player playerSecond)
        {
            if (board.EndState() == true || depth == _simulationDepth)
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

        public void DisplayBoard()
        {
            base.DisplayBoard();
            _boardText.text += $"\n\n P1: {boardEvaluator.Evaluate(_board, _p1)}";
            _boardText.text += $"\n\n P2: {boardEvaluator.Evaluate(_board, _p2)}";
        }
    }
}

