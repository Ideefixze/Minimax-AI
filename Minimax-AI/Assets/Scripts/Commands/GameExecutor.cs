using BoardGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BoardGame
{
    public class GameExecutor : IExecutor
    {
        private Board _board;
        private System.Action _postExecutionAction;

        public GameExecutor(Board board, System.Action postExecution =null)
        {
            _board = board;
            _postExecutionAction = postExecution;
        }

        public void ExecuteCommand(ICommand command)
        {
            Debug.Log("EXECUTING ON THE MAIN BOARD: "+ command.ToString());
            ExecuteCommand(_board, command);
            _postExecutionAction.Invoke();
        }

        public Board ExecuteCommand(Board b, ICommand command)
        {
            command?.Execute(b);
            return b;
        }
    }
}

