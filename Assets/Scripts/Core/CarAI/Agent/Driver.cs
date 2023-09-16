namespace Core.CarAI.Agent
{
    public class Driver
    {
        private readonly ITargetFollow _targetFollow;

        private readonly ITargetFinder _targetFinder;

        private readonly IHitTester[] _hitTesters;

        public float Acceleration { get; private set; }

        public float Brake { get; private set; }

        public float TurnAmount { get; private set; }

        public float SteerSpeed { get; private set; }

        public Mode Mode { get; private set; }

        public Driver(
            ITargetFollow targetFollow,
            ITargetFinder targetFinder,
            IHitTester[] hitTesters)
        {
            _targetFollow = targetFollow;
            _targetFinder = targetFinder;
            _hitTesters = hitTesters;

            Mode = Mode.Driving;
        }

        public void Update(float speed)
        {
            switch (Mode)
            {
                case Mode.Driving:
                    _targetFollow.Update(speed * 0.5f + 7.0f);

                    TurnAmount = _targetFollow.TurnAmount;
                    Brake = _targetFollow.TargetReached ? 1.0f : 0.0f;
                    Acceleration = Brake > 0 ? 0.0f : _targetFollow.ForwardAmount * 0.2f;
                    break;
                case Mode.Idling:
                    Acceleration = 0;
                    Brake = speed > 0 ? 1.0f : 0.0f;
                    break;
                case Mode.Accident:
                    Acceleration = 0;
                    Brake = speed > 0 ? 1.0f : 0.0f;
                    break;
                case Mode.Parking:
                    Acceleration = 0;
                    break;
            }
        }
    }
}