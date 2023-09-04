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

        public void Update()
        {
            _targetFollow.Update();

            TurnAmount = _targetFollow.TurnAmount;
            Acceleration = _targetFollow.ForwardAmount;
        }
    }
}