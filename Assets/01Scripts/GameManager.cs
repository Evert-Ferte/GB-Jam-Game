using System.Collections;
using System.Collections.Generic;
using Game.Terminal;
using UnityEngine;

namespace Game {
    public class GameManager : MonoBehaviour {
        public TerminalController terminalController;
        
        private void Start() {
            StartGame();
        }

        private void StartGame() {
            // 
            terminalController.AddLine("...", 2);
            terminalController.AddLine("...", 2);
            terminalController.AddLine("...", 3);
            terminalController.AddLine("I see you're finally awake", 0.5f);
            terminalController.AddLine("It's been a while", 0.5f);
            terminalController.AddLine("A lot happened while you were gone", 0.5f);
            terminalController.AddLine("You better get going solving the master code", 0.5f);
            terminalController.AddLine("You are our only hope ...", 0.5f);
            terminalController.AddLine(" ", 1);
            terminalController.AddLine("Good luck", 1);
            terminalController.AddLine("God speed", 0.5f);
            terminalController.AddLine(" ", 0.2f);
            terminalController.AddLine(" ", 0.2f);
            terminalController.AddLine(" ", 2);
            terminalController.AddLine("M A S T E R   C O D E", 1);
            terminalController.AddLine("_ _ _ _ _ _ _ _", 0);
        }
    }
}
