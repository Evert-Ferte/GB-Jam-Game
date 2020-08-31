using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Terminal {
    public class TerminalTextLine : MonoBehaviour {
        public Text lineNumber;
        public Text lineText;

        private float timer = 0;

        private UnityEvent animateEvent;
        
        public void SetLineNumber(int number) {
            lineNumber.text = number.ToString();
        }

        public UnityEvent SetLineText(string text) {
            lineText.text = "";
            
            return AnimateText(text);
        }

        private UnityEvent AnimateText(string text) {
            animateEvent = new UnityEvent();
            
            StartCoroutine(TextAnimator(text));

            return animateEvent;
        }

        private IEnumerator TextAnimator(string text) {
            float timePerCharacter = 0.05f;
            int characterIndex = 0;
            
            // Loop until all characters are shown
            while (characterIndex < text.Length) {
                timer += Time.deltaTime;

                if (timer >= timePerCharacter) {
                    timer = 0;
                    characterIndex++;
                    lineText.text = text.Substring(0, characterIndex);
                }
                
                yield return null;
            }
            
            // Invoke all events on animation complete
            animateEvent.Invoke();
        }
    }
}
