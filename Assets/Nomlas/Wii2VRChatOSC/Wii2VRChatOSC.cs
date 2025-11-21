using UnityEngine;

namespace WiiVRC
{
    public class Wii2VRChatOSC : WiiEventListener
    {
        [SerializeField] private OSCServer oscServer;
        public string Param_d_left { get; set; }
        public string Param_d_right { get; set; }
        public string Param_d_up { get; set; }
        public string Param_d_down { get; set; }
        public string Param_a { get; set; }
        public string Param_b { get; set; }
        public string Param_one { get; set; }
        public string Param_two { get; set; }
        public string Param_plus { get; set; }
        public string Param_minus { get; set; }
        public string Param_home { get; set; }

        public override void OnPressed(ButtonType button)
        {
            var param = GetParameter(button);
            if (!string.IsNullOrWhiteSpace(param))
            {
                Debug.Log("Sending OSC: " + param + " => true");
                oscServer.Send(param, true);
            }
        }

        public override void OnReleased(ButtonType button)
        {
            var param = GetParameter(button);
            if (!string.IsNullOrWhiteSpace(param))
            {
                Debug.Log("Sending OSC: " + param + " => false");
                oscServer.Send(param, false);
            }
        }

        public string GetParameter(ButtonType button)
        {
            return button switch
            {
                ButtonType.LEFT => Param_d_left,
                ButtonType.RIGHT => Param_d_right,
                ButtonType.UP => Param_d_up,
                ButtonType.DOWN => Param_d_down,
                ButtonType.A => Param_a,
                ButtonType.B => Param_b,
                ButtonType.ONE => Param_one,
                ButtonType.TWO => Param_two,
                ButtonType.PLUS => Param_plus,
                ButtonType.MINUS => Param_minus,
                ButtonType.HOME => Param_home,
                _ => null
            };
        }
    }
}
