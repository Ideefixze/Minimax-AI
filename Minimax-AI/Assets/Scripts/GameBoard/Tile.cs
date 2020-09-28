using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    public class Tile : IEvaluable
    {
        //Pawn standing on a tile.
        //null means no pawn is currently standing on a tile (empty)
        private Pawn _pawn;
        private Vector2Int _position;
        private float _pointMultiplier;

        public Vector2Int position { get => _position; set => _position = value; }
        public Pawn pawn { get => _pawn; set => _pawn = value; }
        public float pointMultiplier { get => _pointMultiplier; set => _pointMultiplier = value; }

        public Tile(Vector2Int pos, float pointMultilpier=1.0f, Pawn startingPawn=null)
        {
            position = new Vector2Int(pos.x, pos.y);
            pawn = startingPawn;
            _pointMultiplier = pointMultilpier;
        }

        public Tile(Tile t) : this(t.position,t.pointMultiplier,t.pawn)
        {
             
        }

        public bool Occupied()
        {
            if(_pawn==null)
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
            if (pawn == null)
            {
                return 0;
            }
            else
            {
                return (int)(_pawn.Evaluate(player)*_pointMultiplier);
            }
        }

    }
}
