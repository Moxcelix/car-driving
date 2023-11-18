namespace Core.Car
{
    public class Immobilizer
    {
        private readonly Engine _engine;

        public bool IsActive { get; set; }

        public Immobilizer (Engine engine)
        {
            _engine = engine;
        }
    }
}