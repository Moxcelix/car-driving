
namespace Core.Car
{
    public interface ITransmission
    {
        public float Torque { get; }
        public float RPM { get; }
        public float Load { get; }
        public float Brake { get; }
        public int CurrentGear { get; }
        public void SwitchUp();
        public void SwitchDown();
        public void SwitchRight();
        public void SwitchLeft();
        public float GetRatio();
    }
}