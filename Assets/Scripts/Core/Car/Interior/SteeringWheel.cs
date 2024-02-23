using UnityEngine;

namespace Core.Car
{
    public class SteeringWheel : MonoBehaviour
    {
        [SerializeField] private Transform _wheelTransform;

        public float TurnAmount { get; set; }

        private void Update()
        {
            _wheelTransform.localEulerAngles = 
                new Vector3(-TurnAmount * 360.0f * 1.5f, 0);
        }

        public void Steer(float delta)
        {
            TurnAmount += delta;

            TurnAmount = Mathf.Clamp(TurnAmount, -1.0f, 1.0f);
        }
    }
}