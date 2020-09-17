using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    public class PlacePawnCommand : ICommand
    {
        private Pawn _p;
        public PlacePawnCommand(Pawn p)
        {
            _p = p;
        }

        public void Execute(Board b)
        {
            b.PlaceNewPawn(_p);
        }

        public override string ToString()
        {
            return base.ToString() + "   " + _p.position;
        }
    }
}

