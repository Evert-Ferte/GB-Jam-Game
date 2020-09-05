using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Terminal {
    public class TerminalTextLine : MonoBehaviour {
        public TextMeshProUGUI lineNumber;
        public TextMeshProUGUI lineText;

        private string text = "";
        private float waitTime = 0;
        private UnityEvent animateEvent;

        private int visibleCharCounter = 0;
        private bool outOfBounds = false;
        
        public void SetLineNumber(int number) {
            lineNumber.text = number.ToString();
        }

        public void SetLineWaitTime(float waitTime) {
            this.waitTime = waitTime;
        }

        public UnityEvent SetLineText(string text) {
            lineText.text = "";
            this.text = text;
            
            return AnimateText(text);
        }

        private UnityEvent AnimateText(string text) {
            animateEvent = new UnityEvent();
            
            StartCoroutine(TextAnimator(text));

            return animateEvent;
        }

        private IEnumerator TextAnimator(string text) {
            float timer = 0;
            float timePerCharacter = 0.05f; 
            int characterIndex = 0;
            
            // Loop until all characters are shown
            while (characterIndex < text.Length && !outOfBounds) {
                timer += Time.deltaTime;

                if (timer >= timePerCharacter) {
                    timer = 0;
                    characterIndex++;
                    lineText.text = text.Substring(0, characterIndex);

                    CheckIfCharOutOfBounds(characterIndex - 1);
                }

                yield return null;
            }
            
            // Invoke all events on animation complete
            if (!outOfBounds)
                animateEvent.Invoke();
        }
        
        /// <summary>
        /// Checks if the character at the given index is out of the screen.
        /// </summary>
        /// <param name="charIndex">The index of the character to check</param>
        private void CheckIfCharOutOfBounds(int charIndex) {
            lineText.ForceMeshUpdate();
            TMP_TextInfo textInfo = lineText.textInfo;
            TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];

            // Return in the character is not visible
            if (!charInfo.isVisible)
                return;

            // Get the vertices for the current character
            Vector3[] verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
            
            // Create a variable where we will store the char position in
            Vector3 avgCharPos = Vector3.zero;

            // Calculate the char position by getting all 4 vertices of the char mesh and averaging that out
            int startIndex = visibleCharCounter * 4;
            for (int i = startIndex; i < startIndex + 4; i++) {
                avgCharPos += verts[i];
            }
            avgCharPos /= 4;

            // Check if the character is out of bounds, if so, go to the TextOutOfBounds function
            if (avgCharPos.x > lineText.rectTransform.rect.width / 2)
                TextOutOfBounds(charIndex);
            
            // Add 1 to the visible character counter
            visibleCharCounter++;
        }

        /// <summary>
        /// Handles the functionality if a character has been found that if out of bounds.
        /// </summary>
        /// <param name="outOfBoundsIndex">The index of the character that is out of bounds.</param>
        private void TextOutOfBounds(int outOfBoundsIndex) {
            outOfBounds = true;
            
            // Set the text of this line to stop at where the text would not be visible anymore
            lineText.text = text.Substring(0, outOfBoundsIndex);

            // Add the remaining text to a new line
            TerminalController terminal = FindObjectOfType<TerminalController>();
            terminal.ForceAddLine(text.Substring(outOfBoundsIndex), waitTime, animateEvent);
        }
    }
}
