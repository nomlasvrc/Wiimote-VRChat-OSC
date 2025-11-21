using UnityEngine;

namespace WiiVRC
{
    public class WiiEventListener : MonoBehaviour
    {
        protected internal Wii wii;
        public virtual void OnPressed(ButtonType button) { }
        public virtual void OnReleased(ButtonType button) { }
    }
}
