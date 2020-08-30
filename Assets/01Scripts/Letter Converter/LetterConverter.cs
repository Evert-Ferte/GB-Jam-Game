using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game {
    public class LetterConverter : MonoBehaviour {
        public Sprite[] alphabethUpperCase;
        public Sprite[] alphabethLowerCase;

        [Space] public Sprite[] specialCharacters;
        public Sprite[] numbers;

        [Space] public Transform tmpParent;
        public Transform letterPrefab;

        private void Start() {
            string word = "Hello World";
            for (int i = 0; i < word.Length; i++) {
                string letter = word.Substring(i, 1);

                Sprite spr = null;

                if (letter == " ")
                    spr = specialCharacters[specialCharacters.Length - 1];
                else
                    spr = GetLetter(letter);

                Image img = Instantiate(letterPrefab, tmpParent).GetComponent<Image>();
                img.sprite = spr;

                img.gameObject.SetActive(true);
            }
        }

        public Sprite GetLetter(string letter) {
            char c = letter[0];
            return char.IsUpper(c) ? alphabethUpperCase[c - 65] : alphabethLowerCase[char.ToUpper(c) - 65];
        }

        public Sprite GetLetter(int index, bool isUpperCase) {
            if (isUpperCase)
                return alphabethUpperCase[index];
            return alphabethLowerCase[index];
        }

        public Sprite GetNumber(int number) {
            return numbers[number];
        }

        public Sprite GetSpecialCharacter(string letter) {
            return specialCharacters[specialCharacters.Length];
        }
    }
}