namespace Core.Car
{
    public class CentralLocking
    {
        private readonly Door[] _doors;

        public bool Locked { get; set; } 

        public CentralLocking(Door[] doors)
        {
            _doors = doors;
        }

        public void Update()
        {
            foreach (var door in _doors)
            {
                door.IsLocked = Locked;
            }
        }

    }
}