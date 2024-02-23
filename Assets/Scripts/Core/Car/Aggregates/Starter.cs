using System;
using UnityEngine;

namespace Core.Car
{
    public enum EngineState
    {
        STOPED,
        STARTED
    }

    [System.Serializable]
    public class Starter
    {
        private const float c_transitionSpeed = 0.4f;

        [SerializeField] private AnimationCurve _startEngine;

        private float _runningValue;
        private float _runningTransition;

        public bool Ignition { get; private set; } = false;
        public EngineState State { get; private set; } = EngineState.STOPED;
        public float RPMValue => _runningValue;
        public bool IsStarting =>
            State == EngineState.STARTED &&
            _runningTransition > 0.0f &&
            _runningTransition < 1.0f;

        public Action<EngineState> OnChangeState;

        public void LoadSyncState(bool state)
        {
            if(State == EngineState.STOPED == state)
            {
                SwitchState();
            }
        }

        public bool GetSyncState()
        {
            return State == EngineState.STARTED;
        }

        public void SetState(EngineState state)
        {
            if (!Ignition)
            {
                state = EngineState.STOPED;
            }

            if (State == state)
            {
                return;
            }

            State = state;

            OnChangeState?.Invoke(State);
        }

        public void SwitchState()
        {
            Ignition = State == EngineState.STOPED;

            SetState(State == EngineState.STOPED ?
                EngineState.STARTED :
                EngineState.STOPED);
        }

        public void Update()
        {
            if (!Ignition)
            {
                SetState(EngineState.STOPED);
            }

            if (State == EngineState.STARTED && _runningTransition < 1.0f)
            {
                _runningTransition += c_transitionSpeed * Time.deltaTime;
                _runningValue = _startEngine.Evaluate(_runningTransition);
            }
            if (State == EngineState.STOPED && _runningTransition > 0.0f)
            {
                _runningTransition = 0;
                _runningValue = 0;
            }

            _runningTransition = Mathf.Clamp(_runningTransition, 0.0f, 1.0f);
        }
    }
}