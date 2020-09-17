using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    public class Pawn : IEvaluable
    {
        private Vector2Int _position;
        private int _absoluteValue;
        private Player _owner;

        public Vector2Int position { get => _position; set => _position = value; }
        public Player owner { get => _owner; set => _owner = value; }
        public int absoluteValue { get => _absoluteValue; set => _absoluteValue = value; }

        public Pawn(Vector2Int startingPosition, int value, Player owner)
        {
            _position = startingPosition;
            _absoluteValue = value;
            _owner = owner;
        }

        public Pawn(Pawn p) : this(p.position, p.absoluteValue, p.owner)
        {

        }
        

        public int Evaluate(Player player)
        {
            if (_owner == player)
                return _absoluteValue;
            else
                return 0;
        }
    }
}

