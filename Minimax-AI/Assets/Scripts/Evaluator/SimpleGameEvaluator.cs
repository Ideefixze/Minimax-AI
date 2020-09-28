using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    /// <summary>
    /// Evaluator that gives score based on total score for having pawn placed on a tile (no special rules)
    /// </summary>
    public class SimpleGameEvaluator: IEvaluator
    {

        public SimpleGameEvaluator()
        {
            
        }
        public int Evaluate(Board board, Player player)
        {
            int score = 0;
            foreach (Tile t in board.tiles)
            {
                score += t.Evaluate(player);
            }
            return score;
        }
    }
}

