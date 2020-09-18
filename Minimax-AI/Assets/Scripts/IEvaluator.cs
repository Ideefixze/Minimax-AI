using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    public interface IEvaluator
    {
        int Evaluate(Board board, Player p);
    }
}

