using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonSaysTile : MonoBehaviour
{
    public SimonSaysController controller;
    private SpriteRenderer renderer;

    private void Awake() 
    {
        renderer = GetComponent<SpriteRenderer>();    
    }

    private void OnMouseDown() 
    {
        controller.TileClicked(renderer);
    }
}
