namespace Core.CarAI
{
    public class Driver
    {
        private readonly ITargetFollow _targetFollow;

        public float Acceleration { get; private set; }

        public float Brake { get; private set; }

        public float TurnAmount { get; private set; }

        public float SteerSpeed { get; private set; }

        public Driver(ITargetFollow targetFollow)
        {
            _targetFollow = targetFollow;
        }

        public void Update(float speed)
        {
            _targetFollow.Update(speed * 0.5f + 7.0f);

            TurnAmount = _targetFollow.TurnAmount;
            Brake = _targetFollow.TargetReached ? 1.0f : 0.0f;
            Acceleration = Brake > 0 ? _targetFollow.ForwardAmount * 0.2f : 0.0f;
        }
    }
}