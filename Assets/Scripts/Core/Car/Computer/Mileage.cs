namespace Core.Car
{
    public class Mileage
    {
        private float _amount = 0;

        public float Amount { get { return _amount; } }

        public void Update(float meters)
        {
            if (_amount < 0)
            {
                throw new System.ArgumentException();
            }

            _amount += meters;
        }
    }
}