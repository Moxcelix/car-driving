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
        [SerializeField] private RangeLightFixture _highLight;
        [SerializeField] private float _dippedLightRange = 0.7f;

        public HeadLightState LightState { get; private set; } = HeadLightState.DIPPED;

        public bool Enabled { get; set; } = false;

        public void Update()
        {
            _highLight.SetRange(LightState == HeadLightState.HIGH ?
                1.0f : _dippedLightRange);
            _highLight.SetLight(Enabled);
        }

        public void SwitchHighLight()
        {
            LightState = LightState == HeadLightState.DIPPED ?
                HeadLightState.HIGH :
                HeadLightState.DIPPED;
        }
    }
}
