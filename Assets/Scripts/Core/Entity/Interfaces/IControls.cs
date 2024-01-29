namespace Core.Entity
{
    public interface IControls
    {
        public delegate void SingleActionDelegate();

        public SingleActionDelegate Leave { set; }

        public float RotationDeltaX { get; }
        public float RotationDeltaY { get; }
        public float MoveForward { get; }
        public float MoveSide { get; }
        public bool IsRunning { get; }
        public bool IsJumping { get; }
    }
}