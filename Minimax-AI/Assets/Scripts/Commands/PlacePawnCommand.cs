using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    public class PlacePawnCommand : ICommand
    {
        private Pawn pawn;
        public PlacePawnCommand(Pawn p)
        {
            pawn = p;
        }

        public void Execute(Board b)
        {
            b.PlaceNewPawn(pawn);
        }

        public override string ToString()
        {
            return base.ToString() + "   " + pawn.Position;
        }
    }
}

