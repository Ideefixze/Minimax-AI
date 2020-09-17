using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    public interface IExecutor
    {
        void ExecuteCommand(ICommand command);
        Board ExecuteCommand(Board b, ICommand command);
    }
}

