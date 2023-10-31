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
        private const float gasRevard = 0.3f;
        private const float brakeRevard = -0.01f;

        private Hit[] _hits;
        private float _gas;
        private float _brake;

        public float Gas => _gas;

        public float Brake => _brake;


        public void Bump()
        {
            SetReward(bumpRevard);
        }

        public void PositionReach()
        {
            SetReward(positionReachedReward);
        }

        public void UpdateHits(Hit[] hits)
        {
            _hits = hits;
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            if (_hits == null) return;

            foreach (var hit in _hits)
            {
                sensor.AddObservation(hit.Distance);
            }
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            var action = actions.ContinuousActions[0];

            Debug.Log($"Action received : {action}");

            _gas = action > 0 ? action : 0;
            _brake = action < 0 ? -action : 0;

            SetReward(_gas * gasRevard);
            SetReward(_brake * brakeRevard);
        }
    }
}