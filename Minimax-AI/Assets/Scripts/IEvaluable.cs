using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    public interface IEvaluable
    {
        int Evaluate(Player player);
    }
}
