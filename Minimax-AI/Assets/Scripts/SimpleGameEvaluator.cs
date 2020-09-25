using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
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

