using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Terminal {
    public class TerminalController : MonoBehaviour {
        public bool textFromBottom = true;

        [Space] public Transform lineHolder;
        public GameObject terminalTextLinePrefab;

        private List<RectTransform> lineBuffer = new List<RectTransform>();

        private int lineCounter = 0;
        private float lineHolderHeight;

        private bool isWorking = false;
        private List<string> jobQueue = new List<string>();

        private LetterConverter letterConverter;

        private void Start() {
            lineHolderHeight = lineHolder.GetComponent<RectTransform>().rect.height;
        }

        private void Update() {
            // TODO - check if text overflows, then add multiple lines instead of 1 long line
            /*if (Input.GetKeyDown(KeyCode.Space)) {
                AddLine("Lorem ipsum dolor sit amet, consectetur adipiscing elit.");
                AddLine("Duis vitae sem sed ante sagittis egestas vitae hendrit.");
            }*/
        }

        public void AddLine(string newLine) {
            AddLine(new []{newLine});
        }

        public void AddLine(string[] newLines) {
            jobQueue.AddRange(newLines);
            NewLine(jobQueue[0]);
        }

        /// <summary>
        /// Adds a new line into the terminal.
        /// </summary>
        /// <param name="newLine">The line of text to show in the terminal.</param>
        private void NewLine(string newLine) {
            // Return if already working on a line
            if (isWorking)
                return;
            isWorking = true;
            
            jobQueue.RemoveAt(0);
            lineCounter++;
            
            // Spawn the new line in the canvas
            TerminalTextLine line = Instantiate(terminalTextLinePrefab, Vector3.zero, Quaternion.identity, lineHolder)
                .GetComponent<TerminalTextLine>();

            // Get and add the RectTransform component to a buffer containing all lines shown on the screen
            lineBuffer.Add(line.GetComponent<RectTransform>());

            // Set the line number and text attribute
            line.SetLineNumber(lineCounter);
            UnityEvent animationEvent = line.SetLineText(newLine);
            // Add an event listener to the animation event, when completed, add the next line in the job queue
            animationEvent.AddListener(() => {
                isWorking = false;
                if (jobQueue.Count > 0)
                    NewLine(jobQueue[0]);
            });

            // Move all lines to the correct position
            MoveLines();

            // Manage the buffer by deleting lines not shown on the screen
            ManageBuffer();
        }

        /// <summary>
        /// Moves all lines in the terminal to the correct position.
        /// </summary>
        private void MoveLines() {
            int i = lineBuffer.Count;

            // Loop through all lines in the line buffer
            foreach (RectTransform line in lineBuffer) {
                // Get and set the position of the current line based on the position in the line buffer
                float yPos = (textFromBottom ? (45 * i - 45) : lineHolderHeight - 45 - 45 * i + 45);
                line.anchoredPosition = Vector2.up * yPos;
                i--;
            }
        }

        /// <summary>
        /// Manages the buffer by removing and deleting lines currently outside the canvas.
        /// </summary>
        private void ManageBuffer() {
            int i = 0;
            
            List<int> indicesToRemove = new List<int>();

            // Loop through all lines in the line buffer
            foreach (RectTransform line in lineBuffer) {
                // Remove and destroy the current line if it's outside of the canvas
                if (line.anchoredPosition.y >= lineHolderHeight ||
                    line.anchoredPosition.y < lineHolderHeight % 45 - 45) {
                    indicesToRemove.Add(i);
                    Destroy(line.gameObject);
                }
                
                i++;
            }

            // Remove all lines to be removed
            foreach (int index in indicesToRemove) {
                lineBuffer.RemoveAt(index);
            }
        }
    }
}