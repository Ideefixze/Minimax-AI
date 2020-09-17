using BoardGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    public class SimulatedBoard : BoardView
    {
        int turn = 0;
        GameExecutor gameExecutor;
        // Start is called before the first frame update
        void Start()
        {
            base.Start();
            gameExecutor = new GameExecutor(_board, DisplayBoard);
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.S))
            {
                Simulate();
            }

            if(Input.GetKeyDown(KeyCode.A))
            {
                gameExecutor.ExecuteCommand(new PlacePawnCommand(new Pawn(new Vector2Int(0, 0), 1, _p1)));
            }
        }

        private void Simulate()
        {
            
            Player player1; //Player doing this move
            Player player2; //Player next in turn
            if (turn%2==0)
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
            foreach(ICommand move in moves)
            {
                Board afterMove = _board.Copy();
                gameExecutor.ExecuteCommand(afterMove, move);
                DebugBoard(afterMove);
                int score = Minimax(afterMove, 0, player2, player1);
                if(score>bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }
            }

            if(bestMove!=null)
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
            if(board.EndState()==true || depth == 1)
            {
                return  board.Evaluate(playerFirst) - board.Evaluate(playerSecond);
            }

            int bestScore = int.MinValue;
            List<ICommand> moves = board.PossibleMoves(playerFirst);

            foreach(ICommand move in moves)
            {
                Board afterMove = board.Copy();
                gameExecutor.ExecuteCommand(afterMove, move);
                //Debug.Log($"Depth: {depth}");
                //DebugBoard(afterMove);
                int score = Minimax(afterMove, depth + 1, playerSecond, playerFirst);
                bestScore = Mathf.Max(bestScore, score);
            }
            return bestScore;
        }

        public void DebugBoard(Board _board)
        {
            string board = "";
            for (int i = 0; i < _size.x; i++)
            {
                for (int j = 0; j < _size.y; j++)
                {
                    if (_board.tiles[i, j].Occupied() == false)
                    {
                        board += " " + (int)(_board.tiles[i, j].pointMultiplier) + " ";
                    }
                    else
                    {
                        if (_board.tiles[i, j].pawn.owner == _p1)
                        {
                            board += " A ";
                        }
                        else
                        {
                            board += " B ";
                        }
                    }
                }
                board += "\n";
            }

            board += $"\n\nP1 evaluation: {_board.Evaluate(_p1)}\n";
            board += $"P2 evaluation: {_board.Evaluate(_p2)}\n";
            board += $"\n\nPawns:{_board.pawns.Count}\n";

            Debug.Log(board);
        }
    }
}

