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
        
        [Space] public Animator screenFader;

        // Upgraded version: https://stackoverflow.com/questions/3213/convert-integers-to-written-numbers#3267
        // For numbers: https://stackoverflow.com/questions/20156/is-there-an-easy-way-to-create-ordinals-in-c
        private readonly string[] ordinalNumbers = new[] {
            "", "First", "Second", "Third", "Fourth", "Fifth", "Sixth", "Seventh", "Eighth", "Ninth", "Tenth",
            "Eleventh", "Twelfth", "Thirteenth", "Fourteenth", "Fifteenth", "Sixteenth", "Seventeenth", "Eighteenth", "Nineteenth", "Twentieth",
        };

        private int codeLength = 3;
        private int puzzleCounter = 0;

        private string code;

        private UnityEvent onScreenFadedInEvent = new UnityEvent();
        
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
            terminalController.AddLine(" ", 0.1f);
            terminalController.AddLine(" ", 0.1f);
            terminalController.AddLine(" ", 0.1f);
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

            // Show the puzzle number
            terminalController.AddLine(" ", 2);
            terminalController.AddLine(ConvertNumberToWrittenNumber(puzzleCounter) + " puzzle - " + nextPuzzle.name, 0.5f);
            
            // Show the puzzle description
            terminalController.AddLine("Description: " + nextPuzzle.description, 2);
            
            // Show a countdown
            terminalController.AddLine(" ", 0.1f);
            terminalController.AddLine("Ready?", 3);
            terminalController.AddLine("3", 1);
            terminalController.AddLine("2", 1);
            terminalController.AddLine("1", 1);
            
            UnityEvent onPuzzleStartEvent = new UnityEvent();
            onPuzzleStartEvent.AddListener(FadeScreenIn);
            onScreenFadedInEvent.AddListener(() => {
                OnPuzzleStartEvent(nextPuzzle);
            });
            terminalController.AddLine("0", 1, onPuzzleStartEvent);
        }

        private void OnPuzzleStartEvent(Puzzle nextPuzzle) {
            // Hide the terminal
            terminalController.gameObject.SetActive(false);
            
            // Check whether the should be spawn in the UI or not 
            bool isUiBased = nextPuzzle.prefab.GetComponent<RectTransform>() != null;
                
            // Spawn the new puzzle prefab
            Instantiate(nextPuzzle.prefab, isUiBased ? uiBasedPuzzleParent : nonUiBasedPuzzleParent);
        }

        /// <summary>
        /// When called, dialog for when a puzzle has been completed will be shown. Depending on how many puzzles have
        /// been completed, the corresponding code will be shown.
        /// </summary>
        public void PuzzleEnd(GameObject puzzleObject) {
            // Play the fade in animation
            FadeScreenIn();
            
            onScreenFadedInEvent.RemoveAllListeners();
            onScreenFadedInEvent.AddListener(() => {
                // Destroy the puzzle and show the terminal
                terminalController.gameObject.SetActive(true);
                Destroy(puzzleObject);
            
                // Show some spaces first
                terminalController.AddLine(" ", 0.1f);
                terminalController.AddLine(" ", 0.1f);
                terminalController.AddLine(" ", 0.1f);
            
                // Show the dialog for when a puzzle has been completed
                terminalController.AddLine("Excellent", 0.5f);
                terminalController.AddLine("You managed to solve the puzzle", 0.5f);
                terminalController.AddLine("Let's take a look at our code fragment", 2);
                terminalController.AddLine("...", 2);
                terminalController.AddLine(" ", 0.1f);
            
                // Show the master code
                terminalController.AddLine("M A S T E R   C O D E", 1);
                terminalController.AddLine(GetCode(), 0);

                // If the code is not completed, show the dialog for the start of the next puzzle
                if (puzzleCounter < codeLength) {
                    NextPuzzle();
                    return;
                }
            
                GameEnd();
            });
        }

        private void GameEnd() {
            terminalController.AddLine(" ", 0.1f);
            terminalController.AddLine("GAME END - SOME COOL STORY DIALOG HERE", 0.5f);
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

#region Screen fader functions

        /// <summary>
        /// Starts the screen fade in animation.
        /// </summary>
        private void FadeScreenIn() {
            screenFader.gameObject.SetActive(true);
            screenFader.SetTrigger("Fade");
        }

        /// <summary>
        /// Starts the screen fade out animation.
        /// </summary>
        private void FadeScreenOut() {
            screenFader.SetTrigger("Fade");
        }

        public void OnScreenFadedIn(bool isFadingIn) {
            // isFadingIn ? Fade in completed : Fade out started
            if (!isFadingIn) return;
            
            // Invoke the onScreenFadedIn event
            onScreenFadedInEvent.Invoke();

            // Fade back out
            FadeScreenOut();
        }

        public void OnScreenFadedOut(bool isFadingIn) {
            // isFadingIn ? Fade in started : Fade out completed
            if (isFadingIn) return;
            
            // Hide the screen fader
            screenFader.gameObject.SetActive(false);
        }

#endregion
    }
}
