namespace Core.Car
{
    public class DrivingController
    {
        private float _gas;
        private float _brake;
        private float _rpm;

        public void Update(float rpm, float gas, float brake)
        {
            _rpm = rpm;
            _gas = gas;
            _brake = brake;
        }
    }
}