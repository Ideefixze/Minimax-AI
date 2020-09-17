using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    public interface ICommand
    {
        void Execute(Board b);
    }
}

