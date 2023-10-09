using UnityEngine;

namespace Core.Test
{
    public class TestSpeed : MonoBehaviour
    {
        [SerializeField] private Core.Car.Car _car;

        private float _timer = 0;
        private bool _measured = false;

        private void Update()
        {
            if (_car.BrakePedal.Value > 0.5f)
            {
                _timer = 0;
            }

            if (!_measured)
            {
                _timer += Time.deltaTime;
            }

            if (_car.GetSpeed() * 3.6f >= 100.0f)
            {
                Debug.Log(_timer);

                _measured = true;
            }
            else
            {
                _measured = false;
            }
        }
    }
}
