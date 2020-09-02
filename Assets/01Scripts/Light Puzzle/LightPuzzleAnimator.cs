using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.LightPuzzle {
    public class LightPuzzleAnimator : MonoBehaviour {
        public Image[] lights;
        
        [Space] public RectTransform barPointer;
        public Image barImage1;
        public Image barImage2;

        private readonly float[] lightTimers = {0f, 0f, 0f, 0f, 0f, 0f};
        private readonly float[] lightDurations = {3.64f, 6.42f, 5.32f, 2.89f, 8.34f, 2.19f};

        private readonly int barMaxHeight = 41;
        private bool isBarPointerMoving = false;

        private void Update() {
            AnimateLights();
            AnimateBar();
        }
        
        private void AnimateLights() {
            float timeToAdd = Time.deltaTime;

            // Add to the timers
            for (int i = 0; i < lightTimers.Length; i++) {
                lightTimers[i] += timeToAdd;

                // Switch the light state if the timer has expired
                if (lightTimers[i] >= lightDurations[i]) {
                    lightTimers[i] = 0;
                    lights[i].enabled = !lights[i].enabled;
                }
            }
        }

        private void AnimateBar() {
            if (!isBarPointerMoving)
                MoveBarPointer();
        }

        private void MoveBarPointer() { 
            StartCoroutine(BarMover(Random.Range(0, barMaxHeight), Random.Range(5, 30) / 10f));
        }

        private IEnumerator BarMover(int height, float duration) {
            isBarPointerMoving = true;

            float bTimer = 0;
            float distance = height - barPointer.anchoredPosition.y;
            float startYPos = barPointer.anchoredPosition.y;

            while (bTimer < duration) {
                bTimer += Time.deltaTime;
                
                Vector2 newPos = barPointer.anchoredPosition;
                newPos.y = startYPos + /*(duration / height) * */Mathf.Floor(bTimer / (duration / distance));
                
                barPointer.anchoredPosition = new Vector2(newPos.x, newPos.y);

                yield return null;
            }

            barPointer.anchoredPosition = new Vector2(0, height);

            isBarPointerMoving = false;
        }
    }
}
