using OscCore;
using UnityEngine;

namespace WiiVRC
{
    public class OSCServer : MonoBehaviour
    {
        OscClient Client = new OscClient("127.0.0.1", 9000);

        public void Send(string parameterName, int element) => Client.Send($"/avatar/parameters/{parameterName}", element);
        public void Send(string parameterName, float element) => Client.Send($"/avatar/parameters/{parameterName}", element);
        public void Send(string parameterName, bool element) => Client.Send($"/avatar/parameters/{parameterName}", element);
    }
}
