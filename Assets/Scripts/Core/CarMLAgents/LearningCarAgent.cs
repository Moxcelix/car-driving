using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

namespace Core.CarMLAgents
{
    public class LearningCarAgent : Agent
    {
        private const float positionReachedReward = 1.0f;
        private const float bumpRevard = -0.5f;
        private const float gasRevard = 0.05f;
        private const float brakeRevard = -0.01f;
        private const float distanceRevard = -0.05f;
        private const float timeSpendRevard = -0.01f;

        private Hit[] _hits;
        private float _gas;
        private float _brake;
        private float _steer;
        private float _distanceSensetivity;

        public float Gas => _gas;

        public float Brake => _brake;
        
        public void TimeSpend()
        {
            SetReward(timeSpendRevard);
        }

        public void Bump()
        {
            SetReward(bumpRevard);
        }

        public void PositionReach()
        {
            SetReward(positionReachedReward);
        }

        public void UpdateSteer(float steer)
        {
            _steer = steer;
        }

        public void UpdateHits(Hit[] hits)
        {
            _hits = hits;

            foreach(var hit in _hits)
            {
                if(hit.Distance < _distanceSensetivity)
                {
                    SetReward(distanceRevard);
                }
            }
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            if (_hits == null) return;

            foreach (var hit in _hits)
            {
                sensor.AddObservation(hit.Distance);
            }

            sensor.AddObservation(_steer);
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            var action = actions.ContinuousActions[0];

            _gas = action > 0 ? action : 0;
            _brake = action < 0 ? -action : 0;

            _gas = Mathf.Clamp01(_gas);
            _brake = Mathf.Clamp01(_brake);

            _distanceSensetivity = 
                Mathf.Abs(actions.ContinuousActions[1]);

            SetReward(_gas * gasRevard);
            SetReward(_brake * brakeRevard);
        }
    }
}