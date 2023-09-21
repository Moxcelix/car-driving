public interface IDriver
{
    public float Acceleration { get; }

    public float Brake { get; }

    public float TurnAmount { get; }

    public float SteerSpeed { get; }
}