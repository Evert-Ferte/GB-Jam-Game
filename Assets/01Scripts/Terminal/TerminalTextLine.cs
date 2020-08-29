using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Terminal {
    public class TerminalTextLine : MonoBehaviour {
        public Text lineNumber;
        public Text lineText;
        
        public void SetLineNumber(int number) {
            lineNumber.text = number.ToString();
        }

        public void SetLineText(string text) {
            lineText.text = text;
        }
    }
}
