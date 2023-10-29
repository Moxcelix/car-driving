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

        private Hit[] _hits;
        private float _gas;
        private float _brake;

        public float Gas { get { return _gas; } }

        public float Brake { get { return _brake; } }

        public void Bump()
        {
            SetReward(bumpRevard);
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            foreach (var hit in _hits)
            {
                sensor.AddObservation(hit.Distance);
            }
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            Debug.Log("Action received!");

            _gas = actions.ContinuousActions[0];
            _brake = actions.ContinuousActions[1];
        }
    }
}