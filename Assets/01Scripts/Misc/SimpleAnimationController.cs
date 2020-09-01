using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleAnimationController : MonoBehaviour {
    public float duration = 5;
    public GameObject[] objectsToAnimate;

    private float timer = 0;

    private void Update() {
        Animate();
    }

    private void Animate() {
        timer += Time.deltaTime;

        if (timer >= duration) {
            timer = 0;

            // Hide or show the object
            foreach (GameObject o in objectsToAnimate) {
                o.SetActive(!o.activeSelf);
            }
        }
    }
}