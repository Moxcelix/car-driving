using System;
using UnityEngine;

namespace Core.Car
{
    [Serializable]
    public class TurnLights
    {
        [Flags]
        private enum LightState
        {
            NONE = 0,
            LEFT = 1,
            RIGHT = 2,
            BOTH = 4
        }

        [SerializeField] private LightGroup _leftLights;
        [SerializeField] private LightGroup _rightLights;

        private readonly float _turnLightsSpeed = 2.3f;
        private float _turnLightsPhasa = 0;
        private bool _blinkState = false;

        private LightState _lightState = LightState.NONE;

        public Action<bool> OnBlinkerSwitch;

        public void Update()
        {
            if(_lightState == LightState.NONE) 
            {
                ClearPhasa();
            }
            else
            {
                UpdatePhasa();
                SetBlinkState((int)_turnLightsPhasa % 2 == 0);
            }

            ControlFlashing(_rightLights, LightState.RIGHT);
            ControlFlashing(_leftLights, LightState.LEFT);
        }

        public void SwitchBlinker(BlinkerState blinkerState)
        {

            _lightState = (LightState)blinkerState | (_lightState & LightState.BOTH);
        }

        public void SwitchEmergency(bool state)
        {
            if (StateEquals(state, LightState.BOTH))
            {
                return;
            }

            _lightState ^= LightState.BOTH;
        }

        private bool StateEquals(bool turnState, LightState targetState)
        {
            return turnState == ((_lightState & targetState) == targetState);
        }

        private void ControlFlashing(LightGroup lightGroup, LightState state)
        {
            if ((_lightState & state) == state ||
                (_lightState & LightState.BOTH) == LightState.BOTH)
            {
                Flashing(lightGroup);
            }
            else
            {
                lightGroup.SetLight(false);
            }
        }

        private void Flashing(LightGroup lightGroup)
        {
            lightGroup.SetLight(_blinkState);
        }

        private void UpdatePhasa()
        {
            _turnLightsPhasa += _turnLightsSpeed * Time.deltaTime;
        }

        private void ClearPhasa()
        {
            _blinkState = false;
            _turnLightsPhasa = 0;
        }

        private void SetBlinkState(bool state)
        {
            if(state == _blinkState)
            {
                return;
            }
            
            _blinkState = state;

            OnBlinkerSwitch?.Invoke(state);
        }

    }
}