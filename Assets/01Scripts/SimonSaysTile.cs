using UnityEngine;
using UnityEngine.UI;

public class SimonSaysTile : MonoBehaviour
{
    [SerializeField] private Sprite onSprite = null;
    [SerializeField] private Sprite offSprite = null;
    private Image image;

    private void Awake() => image = GetComponent<Image>();
    public void UpdateColor(bool active) => image.sprite = active ? onSprite : offSprite;
    public void SwitchTile(bool state) => image.enabled = state;
}
