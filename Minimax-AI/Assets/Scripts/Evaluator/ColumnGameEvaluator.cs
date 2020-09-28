using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    public class ColumnGameEvaluator: IEvaluator
    {
        private int _columnBonus;

        public ColumnGameEvaluator(int columnBonus)
        {
            _columnBonus = columnBonus;
        }
        public int Evaluate(Board board, Player player)
        {
            int score = 0;
            foreach (Tile t in board.tiles)
            {
                score += t.Evaluate(player);
            }
            //Get bonus score for player one if they make columns
            //Or bonus score for player two if they make horizontal lines
            foreach (Pawn p in board.pawns)
            {
                if (p.owner == player)
                {
                    if(player.id==0)
                    {
                        if (p.position.y < board.size.y - 1)
                        {
                            Pawn under = board.tiles[p.position.x, p.position.y + 1].pawn;
                            if (under != null)
                            {
                                if (under.owner == player)
                                {
                                    score += _columnBonus;
                                }
                            }
                        }
                    }
                    if (player.id == 1)
                    {
                        if (p.position.x < board.size.x - 1)
                        {
                            Pawn under = board.tiles[p.position.x+1, p.position.y].pawn;
                            if (under != null)
                            {
                                if (under.owner == player)
                                {
                                    score += _columnBonus;
                                }
                            }
                        }
                    }
                }
            }
            return score;
        }
    }
}

