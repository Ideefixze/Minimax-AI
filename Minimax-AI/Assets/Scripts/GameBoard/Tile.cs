using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    public class Tile : IEvaluable
    {
        //Pawn standing on a tile.
        //null means no pawn is currently standing on a tile (empty)

        public Vector2Int position { get; set; }
        public Pawn Pawn { get; set; }
        public float PointMultiplier { get; set; }

        public Tile(Vector2Int pos, float pointMultilpier=1.0f, Pawn startingPawn=null)
        {
            position = new Vector2Int(pos.x, pos.y);
            Pawn = startingPawn;
            PointMultiplier = pointMultilpier;
        }

        public Tile(Tile t) : this(t.position,t.PointMultiplier,t.Pawn)
        {
             
        }

        public bool Occupied()
        {
            if(Pawn==null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public int Evaluate(Player player)
        {
            if (Pawn == null)
            {
                return 0;
            }
            else
            {
                return (int)(Pawn.Evaluate(player)*PointMultiplier);
            }
        }

    }
}
