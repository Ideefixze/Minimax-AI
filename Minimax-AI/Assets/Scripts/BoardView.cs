using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BoardGame
{
    public class BoardView : MonoBehaviour
    {
        public Text _boardText;
        public Vector2Int _size;
        public Board _board;
        public Player _p1;
        public Player _p2;

        public void Start()
        {
            _p1 = new Player(1);
            _p2 = new Player(2);
            _board = new Board(_size);

            for(int i = 0; i<_size.x;i++)
            {
                for(int j = 0; j<_size.y;j++)
                {
                    if(i%2==0 && j%2==0)
                    {
                        _board.tiles[i, j].pointMultiplier = 5.0f;
                    }
                }
            }

            DisplayBoard();
            //_board.PlaceNewPawn(new Pawn(new Vector2Int(2, 0), 10, _p1),_board.tiles[2,0]);
            //_board.PlaceNewPawn(new Pawn(new Vector2Int(2, 4), 10, _p2), _board.tiles[2, 4]);
        }

        public void DisplayBoard()
        {
            Debug.Log("Displaying");
            string board = "";
            for (int i = 0; i < _size.x; i++)
            {
                for (int j = 0; j < _size.y; j++)
                {
                    if(_board.tiles[i,j].Occupied()==false)
                    {
                        board += " " + (int)(_board.tiles[i, j].pointMultiplier) + " ";
                    }
                    else
                    {
                        if(_board.tiles[i, j].pawn.owner == _p1)
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

            _boardText.text = board;
        }

    }
}

