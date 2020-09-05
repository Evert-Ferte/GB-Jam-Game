using System.Collections;
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

        private bool isWorking = false;
        private List<Job> jobQueue = new List<Job>();
        
        private readonly float lineHolderHeight = 80;
        private float lineHeight;

        private void Start() {
            lineHeight = terminalTextLinePrefab.GetComponent<RectTransform>().rect.height;
        }

        /// <summary>
        /// Adds a new line to the queue of lines to show on the terminal.
        /// </summary>
        /// <param name="newLine">The new line to be added.</param>
        /// <param name="waitTime">The amount of time to wait after the line has been completed.</param>
        /// <param name="lineEvent">Possible events to run after the line has been shown and the amount waited.</param>
        public void AddLine(string newLine, float waitTime, UnityEvent lineEvent = null) {
            jobQueue.Add(new Job(newLine, waitTime, lineEvent));
            NewLine(jobQueue[0]);
        }

        /// <summary>
        /// Inserts a new line at the first position, instead of adding it to the last position in the queue. This line
        /// will immediately be shown in the terminal. 
        /// </summary>
        /// <param name="newLine">The new line to be added.</param>
        /// <param name="waitTime">the amount of time to wait after the line has been completed.</param>
        /// <param name="lineEvent">Possible events to run after the line has been shown and the amount waited.</param>
        public void ForceAddLine(string newLine, float waitTime, UnityEvent lineEvent = null) {
            isWorking = false;
            jobQueue.Insert(0, new Job(newLine, waitTime, lineEvent));
            NewLine(jobQueue[0]);
        }

        /// <summary>
        /// Adds a new line into the terminal.
        /// </summary>
        /// <param name="lineJob">The line of text to show in the terminal.</param>
        private void NewLine(Job lineJob) {
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
            line.SetLineWaitTime(lineJob.waitTime);
            UnityEvent animationEvent = line.SetLineText(lineJob.text);
            // Add an event listener to the animation event, when completed, add the next line in the job queue
            animationEvent.AddListener(() => {
                isWorking = false;

                lineJob.lineEvent?.Invoke();

                if (jobQueue.Count > 0) {
                    UnityEvent finishEvent = new UnityEvent();
                    finishEvent.AddListener(() => NewLine(jobQueue[0]));
                    
                    StartCoroutine(WaitForSeconds(lineJob.waitTime, finishEvent));
                }
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
                float yPos = (textFromBottom ? (lineHeight * i - lineHeight) : lineHolderHeight - lineHeight - lineHeight * i + lineHeight);
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
                    line.anchoredPosition.y < (lineHolderHeight % lineHeight) - lineHeight) {
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

        private IEnumerator WaitForSeconds(float seconds, UnityEvent finishEvent) {
            yield return new WaitForSeconds(seconds);
            finishEvent.Invoke();
        }
    }

    class Job {
        public string text;
        public float waitTime;
        public UnityEvent lineEvent;

        public Job() { }

        public Job(string text, float waitTime, UnityEvent lineEvent) {
            this.text = text;
            this.waitTime = waitTime;
            this.lineEvent = lineEvent;
        }
    }
}