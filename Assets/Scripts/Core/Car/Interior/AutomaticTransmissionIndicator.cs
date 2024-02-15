using UnityEngine;

namespace Core.Car
{
    public class AutomaticTransmissionIndicator : MonoBehaviour
    {
        [SerializeField] private Car _car;
        [SerializeField] private AutomaticTransmission _transmission;
     
        [SerializeField] private LightFixture _p;
        [SerializeField] private LightFixture _r;
        [SerializeField] private LightFixture _n;
        [SerializeField] private LightFixture _d;
        [SerializeField] private LightFixture _m;

        private void Update()
        {
            var enabled = _car.Engine.Enabled;
            var mode = _transmission.Mode;

            _p.SetLight(mode == AutomaticTransmissionMode.PARKING && enabled);
            _r.SetLight(mode == AutomaticTransmissionMode.REVERSE && enabled);
            _n.SetLight(mode == AutomaticTransmissionMode.NEUTRAL && enabled);
            _d.SetLight(mode == AutomaticTransmissionMode.DRIVING && enabled);
            _m.SetLight(mode == AutomaticTransmissionMode.MANUAL && enabled);
        }
    }
}