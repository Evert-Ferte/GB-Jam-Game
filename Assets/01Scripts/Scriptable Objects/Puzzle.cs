using UnityEngine;

namespace Game.ScriptableObjects {
    [CreateAssetMenu(fileName = "New Puzzle", menuName = "Create New Puzzle", order = 0)]
    public class Puzzle : ScriptableObject {
        public new string name;
        public string description;
        public GameObject prefab;
    }
}
