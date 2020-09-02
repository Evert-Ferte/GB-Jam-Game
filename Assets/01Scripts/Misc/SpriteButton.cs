using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.UI {
    public class SpriteButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
        public UnityEvent onClick;
        
        private Image img;

        private void Start() {
            img = GetComponent<Image>();
        }

        public void OnPointerDown(PointerEventData eventData) { }

        public void OnPointerUp(PointerEventData eventData) {
            // Switch the state of the button
            Color clr = img.color;
            clr = new Color(clr.r, clr.g, clr.b, clr.a == 100 ? 0 : 100);
            img.color = clr;

            // Invoke the unity event
            onClick?.Invoke();
        }
    }
}
