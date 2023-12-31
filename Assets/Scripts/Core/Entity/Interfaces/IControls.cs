namespace Core.Entity
{
    public interface IControls
    {
        public delegate void SingleActionDelegate();

        public SingleActionDelegate Leave { set; }

        public float RotationDeltaX { get; }
        public float RotationDeltaY { get; }
        public bool MoveForward { get; }
        public bool MoveBack { get; }
        public bool MoveRight { get; }
        public bool MoveLeft { get; }
        public bool IsRunning { get; }
        public bool IsJumping { get; }
    }
}