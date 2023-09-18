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
        [SerializeField] private AnimationCurve _stopEngine;

        private float _runningValue;
        private float _runningTransition;

        public EngineState State { get; private set; } = EngineState.STOPED;
        public float RPMValue => _runningValue;
        public bool IsStarting =>
            State == EngineState.STARTED &&
            _runningTransition > 0.0f &&
            _runningTransition < 1.0f;

        public Action<EngineState> OnChangeState;

        public void SetState(EngineState state)
        {
            if (State == state)
            {
                return;
            }

            State = state;

            OnChangeState?.Invoke(State);
        }

        public void SwitchState()
        {
            SetState(State == EngineState.STOPED ?
                EngineState.STARTED :
                EngineState.STOPED);
        }

        public void Update()
        {
            if (State == EngineState.STARTED && _runningTransition < 1.0f)
            {
                _runningTransition += c_transitionSpeed * Time.deltaTime;
                _runningValue = _startEngine.Evaluate(_runningTransition);
            }
            if (State == EngineState.STOPED && _runningTransition > 0.0f)
            {
                _runningTransition -= c_transitionSpeed * Time.deltaTime;
                _runningValue = _stopEngine.Evaluate(_runningTransition);
            }

            _runningTransition = Mathf.Clamp(_runningTransition, 0.0f, 1.0f);
        }
    }
}