using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    public interface IExecutor
    {
        /// <summary>
        /// Default execute.
        /// </summary>
        /// <param name="command">Command to execute</param>
        void ExecuteCommand(ICommand command);

        /// <summary>
        /// Execute on specific board
        /// </summary>
        /// <param name="board">Board</param>
        /// <param name="command">Comman</param>
        /// <returns>Changed Board</returns>
        Board ExecuteCommand(Board board, ICommand command);
    }
}

