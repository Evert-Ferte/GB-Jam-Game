using UnityEngine;
using UnityEngine.UI;

namespace Game.LightPuzzle {
    public class LightPuzzleController : MonoBehaviour {
        public Image[] lights;
        
        public Image pressureIndicator;
        public Sprite[] pressurePointers;
        
        private int gridSize = 3;

        private bool isGameFinished = false;

        private void Start() {
            int minLit = 2;
            int maxLit = 6;

            int lit = Random.Range(minLit, maxLit + 1);

            bool[] taken = new bool[gridSize * gridSize];
            for (int i = 0; i < lit; i++) {
                int r = Random.Range(0, lights.Length);

                int endlessLoopPreventer = 0;
                while (taken[r]) {
                    r++;
                    endlessLoopPreventer++;

                    if (endlessLoopPreventer >= lights.Length)
                        break;
                }

                lights[r].enabled = true;
            }
            
            CheckGameState();
        }
        
        public void ButtonClickEvent(int btnIndex) {
            if (isGameFinished) return;
            
            Vector2 clickedPos = ConvertIndexToPos(btnIndex);
            
            Vector2 up = clickedPos + Vector2.up;
            Vector2 down = clickedPos + Vector2.down;
            Vector2 left = clickedPos + Vector2.left;
            Vector2 right = clickedPos + Vector2.right;

            int upIndex = ConvertPosToIndex(up);
            int downIndex = ConvertPosToIndex(down);
            int leftIndex = ConvertPosToIndex(left);
            int rightIndex = ConvertPosToIndex(right);
            
            if (InBounds(btnIndex))
                lights[btnIndex].enabled = !lights[btnIndex].enabled;
            if (InBounds(upIndex) && IsNeighbour(btnIndex, upIndex))
                lights[upIndex].enabled = !lights[upIndex].enabled;
            if (InBounds(downIndex) && IsNeighbour(btnIndex, downIndex))
                lights[downIndex].enabled = !lights[downIndex].enabled;
            if (InBounds(leftIndex) && IsNeighbour(btnIndex, leftIndex))
                lights[leftIndex].enabled = !lights[leftIndex].enabled;
            if (InBounds(rightIndex) && IsNeighbour(btnIndex, rightIndex))
                lights[rightIndex].enabled = !lights[rightIndex].enabled;

            CheckGameState();
        }

        private void CheckGameState() {
            bool finished = true;

            int pressureIndex = 0;
            
            foreach (Image l in lights) {
                finished = finished && l.enabled;

                if (!l.enabled)
                    pressureIndex++;
            }

            pressureIndicator.sprite = pressurePointers[pressureIndex];
            
            isGameFinished = finished;

            if (isGameFinished)
                FindObjectOfType<GameManager>().PuzzleEnd(this.gameObject);
        }

        private Vector2 ConvertIndexToPos(int index) {
            if (index < 0 || index >= gridSize * gridSize) return Vector2.zero;
            
            return new Vector2(index % gridSize, Mathf.FloorToInt((float)index / gridSize));
        }

        private int ConvertPosToIndex(Vector2 pos) {
            return Mathf.RoundToInt(pos.y * 3 + pos.x);
        }

        private bool InBounds(int index) {
            return index >= 0 && index < gridSize * gridSize;
        }

        private bool IsNeighbour(int current, int neighbour) {
            return ConvertIndexToPos(current).y == ConvertIndexToPos(neighbour).y ||
                ConvertIndexToPos(current).x == ConvertIndexToPos(neighbour).x;
        }
    }
}
