using UnityEngine;

public class SimonSaysTile : MonoBehaviour
{
    [SerializeField] private SimonSaysController controller = null;
    [SerializeField] private Color highColor = new Color();
    [SerializeField] private Color baseColor = new Color();
    private SpriteRenderer sr;

    private void Awake() 
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void UpdateColor(bool active)
	{
        sr.color = active ? highColor : baseColor;
	}

    private void OnMouseDown()
    {
        controller.OnTileClicked?.Invoke(this);
    }

    public void SwitchTile(bool state)
	{
        sr.enabled = state;
	}
}
