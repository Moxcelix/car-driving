using System;
using UnityEngine;

namespace Core.Car
{
    [Serializable]
    public class TurnLights
    {
        [SerializeField] private LightGroup _leftLights;
        [SerializeField] private LightGroup _rightLights;

        private readonly float _turnLightsSpeed = 2.3f;
        private float _turnLightsPhasa = 0;
        private bool _blinkState = false;

        public Action<bool> OnBlinkerSwitch;

        private BlinkerState _blinkerState = BlinkerState.None;
        private bool _emergencyState = false;

        public void Update()
        {
            if(_blinkerState == BlinkerState.None && !_emergencyState) 
            {
                ClearPhasa();
            }
            else
            {
                UpdatePhasa();
                SetBlinkState((int)_turnLightsPhasa % 2 == 0);
            }

            ControlFlashing(_rightLights, BlinkerState.Rigth);
            ControlFlashing(_leftLights, BlinkerState.Left);
        }

        public void SwitchBlinker(BlinkerState blinkerState)
        {
            _blinkerState = blinkerState;
        }

        public void SwitchEmergency(bool state)
        {
            _emergencyState = state;
        }

        private void ControlFlashing(LightGroup lightGroup, BlinkerState blinkerState)
        {
            if (_blinkerState == blinkerState || _emergencyState)
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