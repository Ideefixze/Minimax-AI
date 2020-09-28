using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    /// <summary>
    /// Basic interface that evaluates board for a given player.
    /// </summary>
    public interface IEvaluator
    {
        int Evaluate(Board board, Player p);
    }
}

