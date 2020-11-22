using BoardGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    public class GameExecutor : IExecutor
    {
        private Board board;
        private System.Action postExecutionAction;

        public GameExecutor(Board board, System.Action postExecution =null)
        {
            this.board = board;
            postExecutionAction = postExecution;
        }
        /// <summary>
        /// Executes command on the main board, specified in _board.
        /// </summary>
        /// <param name="command">Command</param>
        public void ExecuteCommand(ICommand command)
        {
            ExecuteCommand(board, command);
            postExecutionAction.Invoke();
        }

        /// <summary>
        /// Executes command on given board.
        /// </summary>
        /// <param name="b">Board</param>
        /// <param name="command">Command</param>
        /// <returns></returns>
        public Board ExecuteCommand(Board b, ICommand command)
        {
            command?.Execute(b);
            return b;
        }
    }
}

