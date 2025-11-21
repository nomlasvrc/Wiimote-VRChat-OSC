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

            if (wiimote.Button.d_left != d_left) OnChange(wiimote.Button.d_left, ButtonType.LEFT); d_left = wiimote.Button.d_left;
            if (wiimote.Button.d_right != d_right) OnChange(wiimote.Button.d_right, ButtonType.RIGHT); d_right = wiimote.Button.d_right;
            if (wiimote.Button.d_up != d_up) OnChange(wiimote.Button.d_up, ButtonType.UP); d_up = wiimote.Button.d_up;
            if (wiimote.Button.d_down != d_down) OnChange(wiimote.Button.d_down, ButtonType.DOWN); d_down = wiimote.Button.d_down;
            if (wiimote.Button.a != a) OnChange(wiimote.Button.a, ButtonType.A); a = wiimote.Button.a;
            if (wiimote.Button.b != b) OnChange(wiimote.Button.b, ButtonType.B); b = wiimote.Button.b;
            if (wiimote.Button.one != one) OnChange(wiimote.Button.one, ButtonType.ONE); one = wiimote.Button.one;
            if (wiimote.Button.two != two) OnChange(wiimote.Button.two, ButtonType.TWO); two = wiimote.Button.two;
            if (wiimote.Button.plus != plus) OnChange(wiimote.Button.plus, ButtonType.PLUS); plus = wiimote.Button.plus;
            if (wiimote.Button.minus != minus) OnChange(wiimote.Button.minus, ButtonType.MINUS); minus = wiimote.Button.minus;
            if (wiimote.Button.home != home) OnChange(wiimote.Button.home, ButtonType.HOME); home = wiimote.Button.home;
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

        private void OnChange(bool pressed, ButtonType button)
        {
            if (pressed)
            {
                m_OnPressed(button);
            }
            else
            {
                m_OnReleased(button);
            }
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
