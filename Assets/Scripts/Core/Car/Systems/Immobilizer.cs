namespace Core.Car
{
    public class Immobilizer
    {
        private readonly Engine _engine;

        public bool IsActive { get; set; } = false;

        public Immobilizer (Engine engine)
        {
            _engine = engine;
        }

        public void Update()
        {
            if (IsActive)
            {
                if (_engine.Starter.IsStarting)
                {
                    _engine.Starter.SetState(EngineState.STOPED);
                }
            }
        }
    }
}