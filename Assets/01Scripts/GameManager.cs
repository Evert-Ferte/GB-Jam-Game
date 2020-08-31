using System.Collections;
using System.Collections.Generic;
using Game.ScriptableObjects;
using Game.Terminal;
using UnityEngine;
using UnityEngine.Events;

namespace Game {
    public class GameManager : MonoBehaviour {
        public TerminalController terminalController;

        [Space] public Puzzle[] puzzles;

        [Space] public Transform uiBasedPuzzleParent;
        public Transform nonUiBasedPuzzleParent;

        // Upgraded version: https://stackoverflow.com/questions/3213/convert-integers-to-written-numbers#3267
        // For numbers: https://stackoverflow.com/questions/20156/is-there-an-easy-way-to-create-ordinals-in-c
        private readonly string[] ordinalNumbers = new[] {
            "", "First", "Second", "Third", "Fourth", "Fifth", "Sixth", "Seventh", "Eighth", "Ninth", "Tenth",
            "Eleventh", "Twelfth", "Thirteenth", "Fourteenth", "Fifteenth", "Sixteenth", "Seventeenth", "Eighteenth", "Nineteenth", "Twentieth",
        };

        private int codeLength = 8;
        private int puzzleCounter = 0;

        private string code;
        
        private void Start() {
            GenerateCode();
            
            PlayStartSequence();
        }

        /// <summary>
        /// Generates a random code.
        /// </summary>
        private void GenerateCode() {
            for (int i = 0; i < codeLength; i++) {
                int rNumber = Random.Range(0, 10);
                code += rNumber;
            }
        }

        /// <summary>
        /// Plays the starting sequence in the terminal window. After the starting sequence has been shown, the first
        /// puzzle will begin playing.
        /// </summary>
        private void PlayStartSequence() {
            // Starting dialog
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
            terminalController.AddLine("God speed", 2);
            terminalController.AddLine(" ", 0.2f);
            terminalController.AddLine(" ", 0.2f);
            terminalController.AddLine(" ", 0.2f);
            terminalController.AddLine("M A S T E R   C O D E", 1);
            terminalController.AddLine(GetCode(), 0);
            
            // First puzzle dialog
            NextPuzzle();
        }

        /// <summary>
        /// Shows the dialog in the terminal window for the first puzzle. Also starts a random puzzle after the dialog
        /// has been shown.
        /// </summary>
        private void NextPuzzle() {
            puzzleCounter++;

            int puzzleIndex = Random.Range(0, puzzles.Length);
            Puzzle nextPuzzle = puzzles[puzzleIndex];

            UnityEvent onPuzzleStartEvent = new UnityEvent();
            onPuzzleStartEvent.AddListener(() => {
                // Start the puzzle and hide the terminal
                terminalController.gameObject.SetActive(false);
        
                GameObject puzzleObject = Instantiate(nextPuzzle.prefab);
                bool isUiBased = puzzleObject.GetComponent<RectTransform>() != null;
                puzzleObject.transform.SetParent(isUiBased ? uiBasedPuzzleParent : nonUiBasedPuzzleParent);
            });

            // Show the puzzle number
            terminalController.AddLine(" ", 2);
            terminalController.AddLine(ConvertNumberToWrittenNumber(puzzleCounter) + " puzzle - " + nextPuzzle.name, 0.5f);
            
            // Show the puzzle description
            terminalController.AddLine("Description: " + nextPuzzle.description, 2);
            
            // Show a countdown
            terminalController.AddLine(" ", 0.2f);
            terminalController.AddLine("Ready?", 3);
            terminalController.AddLine("3", 1);
            terminalController.AddLine("2", 1);
            terminalController.AddLine("1", 1);
            terminalController.AddLine("0", 1, onPuzzleStartEvent);
        }

        /// <summary>
        /// When called, dialog for when a puzzle has been completed will be shown. Depending on how many puzzles have
        /// been completed, the corresponding code will be shown.
        /// </summary>
        private void PuzzleEnd() {
            // Show the dialog for when a puzzle has been completed
            terminalController.AddLine("Excellent", 0.5f);
            terminalController.AddLine("You managed to solve the puzzle", 0.5f);
            terminalController.AddLine("Let's take a look at our code fragment", 2);
            terminalController.AddLine("...", 2);
            
            // Show the master code
            terminalController.AddLine("M A S T E R   C O D E", 1);
            terminalController.AddLine(GetCode(), 0);
        }
        
        /// <summary>
        /// Converts an integer to the written version of it in string format.
        /// </summary>
        /// <param name="number">The number to be converted.</param>
        /// <returns>Returns the converted number in string format.</returns>
        private string ConvertNumberToWrittenNumber(int number)  {
            if (number <= 0 || number >= ordinalNumbers.Length) {
                // throw new IndexOutOfRangeException("Make sure to give a number between 1 and " + (ordinalNumbers.Length - 1));
                return number.ToString();
            }
            
            return ordinalNumbers[number];
        }

        /// <summary>
        /// Get the code with some amount of numbers revealed, depending on how many puzzles have been completed.
        /// </summary>
        /// <returns>Returns the code for the current game.</returns>
        private string GetCode() {
            string ret = "";
            
            for (int i = codeLength; i > 0; i--) {
                if (i > codeLength - puzzleCounter)
                    ret += code.Substring(i - 1, 1) + " ";
                else
                    ret += "_ ";
            }
            
            return ret;
        }
    }
}
