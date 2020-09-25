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
            _p1 = new Player(0);
            _p2 = new Player(1);
            _board = new Board(_size);

            for(int i = 0; i<_size.x;i++)
            {
                for(int j = 0; j<_size.y;j++)
                {
                    _board.tiles[i, j].pointMultiplier = (int)Random.Range(2,6);
                }
            }

            
        }

        public void DisplayBoard()
        {
            string board = "";
            for (int i = 0; i < _size.y; i++)
            {
                for (int j = 0; j < _size.x; j++)
                {
                    if(_board.tiles[j,i].Occupied()==false)
                    {
                        board += " " + (int)(_board.tiles[j, i].pointMultiplier) + " ";
                    }
                    else
                    {
                        if(_board.tiles[j, i].pawn.owner == _p1)
                        {
                            board += " <color=red>"+(int)(_board.tiles[j, i].pointMultiplier)+"</color> ";
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
        }

    }
}

