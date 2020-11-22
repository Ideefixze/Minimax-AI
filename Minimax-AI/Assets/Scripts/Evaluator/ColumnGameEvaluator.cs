using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    public class ColumnGameEvaluator: IEvaluator
    {
        private int columnBonus;

        public ColumnGameEvaluator(int columnBonus)
        {
            this.columnBonus = columnBonus;
        }
        public int Evaluate(Board board, Player player)
        {
            int score = 0;
            foreach (Tile t in board.Tiles)
            {
                score += t.Evaluate(player);
            }
            //Get bonus score for player one if they make columns
            //Or bonus score for player two if they make horizontal lines
            foreach (Pawn p in board.Pawns)
            {
                if (p.Owner == player)
                {
                    if(player.Id==0)
                    {
                        if (p.Position.y < board.Size.y - 1)
                        {
                            Pawn under = board.Tiles[p.Position.x, p.Position.y + 1].Pawn;
                            if (under != null)
                            {
                                if (under.Owner == player)
                                {
                                    score += columnBonus;
                                }
                            }
                        }
                    }
                    if (player.Id == 1)
                    {
                        if (p.Position.x < board.Size.x - 1)
                        {
                            Pawn under = board.Tiles[p.Position.x + 1, p.Position.y].Pawn;
                            if (under != null)
                            {
                                if (under.Owner == player)
                                {
                                    score += columnBonus;
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

