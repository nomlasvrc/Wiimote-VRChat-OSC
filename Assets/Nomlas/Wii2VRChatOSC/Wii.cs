using UnityEngine;
using WiimoteApi;

namespace WiiVRC
{
    public class Wii : MonoBehaviour
    {
        [SerializeField] private WiiEventListener listener;
        private Wiimote wiimote;
        private bool d_left;
        private bool d_right;
        private bool d_up;
        private bool d_down;
        private bool a;
        private bool b;
        private bool one;
        private bool two;
        private bool plus;
        private bool minus;
        private bool home;
        public bool Initialized { get; private set; }
        public void Find() => WiimoteManager.FindWiimotes();

        private void Start()
        {
            Initialized = false;
            listener.wii = this;
            Find();
        }
        public void Cleanup()
        {
            if (wiimote != null)
            {
                WiimoteManager.Cleanup(wiimote);
                wiimote = null;
            }
        }

        void Update()
        {
            if (!WiimoteManager.HasWiimote()) { return; }
            
            wiimote = WiimoteManager.Wiimotes[0];
            if (wiimote == null) return;

            int ret;
            do
            {
                ret = wiimote.ReadWiimoteData();
            } while (ret > 0);

            if (!Initialized)
            {
                wiimote.SendPlayerLED(true, false, false, false);
                wiimote.SendDataReportMode(InputDataType.REPORT_BUTTONS);
                wiimote.SendStatusInfoRequest();
                Debug.Log("Wiimote Initialized");
                Initialized = true;
            }

            CheckButton(ref d_left, wiimote.Button.d_left, ButtonType.LEFT);
            CheckButton(ref d_right, wiimote.Button.d_right, ButtonType.RIGHT);
            CheckButton(ref d_up, wiimote.Button.d_up, ButtonType.UP);
            CheckButton(ref d_down, wiimote.Button.d_down, ButtonType.DOWN);
            CheckButton(ref a, wiimote.Button.a, ButtonType.A);
            CheckButton(ref b, wiimote.Button.b, ButtonType.B);
            CheckButton(ref one, wiimote.Button.one, ButtonType.ONE);
            CheckButton(ref two, wiimote.Button.two, ButtonType.TWO);
            CheckButton(ref plus, wiimote.Button.plus, ButtonType.PLUS);
            CheckButton(ref minus, wiimote.Button.minus, ButtonType.MINUS);
            CheckButton(ref home, wiimote.Button.home, ButtonType.HOME);
        }

        private void CheckButton(ref bool prev, bool current, ButtonType type)
        {
            if (prev != current)
            {
                if (current)
                    m_OnPressed(type);
                else
                    m_OnReleased(type);
            }
            prev = current;
        }

        public void Identify()
        {
            if (wiimote != null)
            {
                wiimote.RumbleOn = true;
                wiimote.SendStatusInfoRequest();
                Invoke(nameof(StopRumble), 1f);
            }
        }

        private void StopRumble()
        {
            wiimote.RumbleOn = false;
            wiimote.SendStatusInfoRequest();
        }

        private void m_OnPressed(ButtonType button) => listener?.OnPressed(button);
        private void m_OnReleased(ButtonType button) => listener?.OnReleased(button);

        private void OnDestroy()
        {
            Cleanup();
        }
    }

    public enum ButtonType
    {
        LEFT,
        RIGHT,
        UP,
        DOWN,
        A,
        B,
        ONE,
        TWO,
        PLUS,
        MINUS,
        HOME
    }
}
