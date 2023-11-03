using System.ComponentModel;

namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class IsExternalInit { }
}

public record StatisticsRecord(
    float Speed,
    float Gas,
    float Brake,
    float RPM,
    float SteerAmount);
