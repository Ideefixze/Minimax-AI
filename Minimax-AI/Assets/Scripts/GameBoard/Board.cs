using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    public class Board
    {
        private Vector2Int _size;

        private List<Pawn> _pawns;
        private Tile[,] _tiles;
        private IEvaluator evaluator;

        public List<Pawn> pawns { get => _pawns; }
        public Tile[,] tiles { get => _tiles;}
        public Vector2Int size { get => _size; set => _size = value; }


        public Board(Vector2Int boardSize)
        {
            _size = new Vector2Int(boardSize.x, boardSize.y);

            _tiles = new Tile[size.x,size.y];

            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    _tiles[i, j] = new Tile(new Vector2Int(i, j), 1f, null);
                }
            }

            _pawns = new List<Pawn>();
        }
        
        public Board Copy()
        {
            Board b = new Board(this.size);

            //Making copies
            for (int i = 0; i < b.size.x; i++)
            {
                for (int j = 0; j < b.size.y; j++)
                {
                    b.tiles[i, j] = new Tile(new Vector2Int(i, j), this.tiles[i, j].pointMultiplier, null);
                }
            }

            foreach (Pawn p in this.pawns)
            {
                Pawn newPawn = new Pawn(p);
                b.pawns.Add(newPawn);
                b.tiles[p.position.x, p.position.y].pawn = newPawn;
            }

            return b;

        }

        public bool RepositionPawn(Player p, Tile a, Tile b)
        {
            if(a.pawn.owner==p)
            {
                if(b.Occupied() == false)
                {
                    b.pawn = a.pawn;
                    a.pawn = null;
                    b.pawn.position = new Vector2Int(b.position.x, b.position.y);
                    
                    return true;
                }
            }
           
            return false;
        }

        public bool PlaceNewPawn(Pawn pawn)
        {
            if (tiles[pawn.position.x,pawn.position.y].Occupied()==false)
            {
                pawns.Add(pawn);
                tiles[pawn.position.x, pawn.position.y].pawn = pawn;
                return true;
            }
            return false;
        }

        public bool EndState()
        {
            bool end = true;
            foreach (Tile t in tiles)
            {
                if(t.Occupied()==false)
                {
                    return false;
                }
            }
            return end;
        }

        //TODO: Interface similar to IEvaluator, that gives a list of possible moves for a given board
        // So it is more OOP and would be able to create different "games" that have different rules
        public List<ICommand> PossibleMoves(Player p)
        {
            List<ICommand> possibleMoves = new List<ICommand>();

            foreach (Tile t in tiles)
            {
                if (t.Occupied() == false)
                {
                    possibleMoves.Add(new PlacePawnCommand(new Pawn(t.position, 1, p)));
                }
            }

            return possibleMoves;
        }


    }
}

