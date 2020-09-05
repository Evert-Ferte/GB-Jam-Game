using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class ScreenFader : MonoBehaviour {
    public GameManager gameManager;

    private bool isAnimating = false;

    public void OnFadedIn() {
        isAnimating = !isAnimating;
        
        gameManager.OnScreenFadedIn(!isAnimating);
    }

    public void OnFadedOut() {
        isAnimating = !isAnimating;
        
        gameManager.OnScreenFadedOut(isAnimating);   
    }
}
