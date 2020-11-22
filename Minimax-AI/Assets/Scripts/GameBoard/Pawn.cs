using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    public class Pawn : IEvaluable
    {

        public Vector2Int Position { get; set; }
        public Player Owner { get; set; }
        public int AbsoluteValue { get; set; }

        public Pawn(Vector2Int startingPosition, int value, Player owner)
        {
            Position = startingPosition;
            AbsoluteValue = value;
            Owner = owner;
        }

        public Pawn(Pawn p) : this(p.Position, p.AbsoluteValue, p.Owner)
        {

        }
        

        public int Evaluate(Player player)
        {
            if (Owner == player)
                return AbsoluteValue;
            else
                return 0;
        }
    }
}

