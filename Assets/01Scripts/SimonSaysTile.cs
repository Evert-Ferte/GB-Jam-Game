using UnityEngine;

public class SimonSaysTile : MonoBehaviour
{
    [SerializeField] private SimonSaysController controller;
    [SerializeField] private Color highColor;
    [SerializeField] private Color lowColor;
    [SerializeField] private Color baseColor;
    private SpriteRenderer renderer;

    private void Awake() 
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateColor(bool active)
	{
        renderer.color = active ? highColor : baseColor;
	}

    private void OnMouseDown()
    {
        controller.OnTileClicked?.Invoke(this);
    }

    public void SwitchTile(bool state)
	{
        renderer.enabled = state;
	}

	//private void OnMouseUp()
	//{
 //       if (controller.SequencePlaying) return;
 //       renderer.color = baseColor;
	//}

	//private void OnMouseEnter()
	//{
 //       if (controller.SequencePlaying) return;
 //       renderer.color = lowColor;
	//}

	//private void OnMouseExit()
	//{
 //       renderer.color = baseColor;
	//}
}
