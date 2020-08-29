using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Terminal {
    public class TerminalController : MonoBehaviour {
        public bool textFromBottom = true;
        [Space]
        
        public Transform lineHolder;
        public GameObject terminalTextLinePrefab;
        
        private List<string> lines = new List<string>();
        
        void Start() {
            
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                AddLine("hello world");
            }
        }

        private void AddLine(string newLine) {
            // TODO - check if text overflows, then add multiple lines instead of 1 long line
            
            // Add the line to a list of lines
            lines.Add(newLine);

            // Spawn the lines
            Vector3 pos = new Vector3(0, -45 + 45 * lines.Count, 0);
            if (!textFromBottom)
                pos = new Vector3(0, 483 - (-45 + 45 * lines.Count), 0);
            
            TerminalTextLine line = Instantiate(terminalTextLinePrefab, Vector3.zero, Quaternion.identity, lineHolder).GetComponent<TerminalTextLine>();

            // Apply the correct position
            line.GetComponent<RectTransform>().anchoredPosition = pos;
            
            // Set the line number and text
            line.SetLineNumber(lines.Count);
            line.SetLineText(newLine);
        }
    }
}
