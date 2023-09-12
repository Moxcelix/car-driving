using UnityEngine;

namespace Core.Car
{
    public enum HeadLightState : int
    {
        DIPPED = 0,
        HIGH = 1
    }

    [System.Serializable]
    public class HeadLights
    {
        [SerializeField] private LightFixture _highLight;

        public HeadLightState State { get; set; }

        public HeadLights()
        {
            State = HeadLightState.DIPPED;
        }

        public void Update()
        {
            _highLight.SetLight(State == HeadLightState.HIGH);
        }

        public void SwitchHighLight(bool state)
        {
            State = state ? HeadLightState.HIGH : HeadLightState.DIPPED;
        }
    }
}
