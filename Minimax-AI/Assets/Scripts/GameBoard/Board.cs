using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    public class Board
    {

        private IEvaluator evaluator;

        public List<Pawn> Pawns { get; set; }
        public Tile[,] Tiles { get; set; }
        public Vector2Int Size { get; private set; }


        public Board(Vector2Int boardSize)
        {
            Size = new Vector2Int(boardSize.x, boardSize.y);

            Tiles = new Tile[Size.x,Size.y];

            for (int i = 0; i < Size.x; i++)
            {
                for (int j = 0; j < Size.y; j++)
                {
                    Tiles[i, j] = new Tile(new Vector2Int(i, j), 1f, null);
                }
            }

            Pawns = new List<Pawn>();
        }
        
        public Board Copy()
        {
            Board b = new Board(this.Size);

            //Making copies
            for (int i = 0; i < b.Size.x; i++)
            {
                for (int j = 0; j < b.Size.y; j++)
                {
                    b.Tiles[i, j] = new Tile(new Vector2Int(i, j), this.Tiles[i, j].PointMultiplier, null);
                }
            }

            foreach (Pawn p in this.Pawns)
            {
                Pawn newPawn = new Pawn(p);
                b.Pawns.Add(newPawn);
                b.Tiles[p.Position.x, p.Position.y].Pawn = newPawn;
            }

            return b;

        }

        public bool RepositionPawn(Player p, Tile a, Tile b)
        {
            if(a.Pawn.Owner==p)
            {
                if(b.Occupied() == false)
                {
                    b.Pawn = a.Pawn;
                    a.Pawn = null;
                    b.Pawn.Position = new Vector2Int(b.position.x, b.position.y);
                    
                    return true;
                }
            }
           
            return false;
        }

        public bool PlaceNewPawn(Pawn pawn)
        {
            if (Tiles[pawn.Position.x,pawn.Position.y].Occupied()==false)
            {
                Pawns.Add(pawn);
                Tiles[pawn.Position.x, pawn.Position.y].Pawn = pawn;
                return true;
            }
            return false;
        }

        public bool EndState()
        {
            bool end = true;
            foreach (Tile t in Tiles)
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

            foreach (Tile t in Tiles)
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

