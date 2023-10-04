using UnityEngine;

public class SmoothPressing
{
    const float sensitivity = 0.01f;

    private readonly float _pressSpeed = 0.1f;
    private readonly float _releaseSpeed = 0.1f;

    public float Value { get; private set; }

    public SmoothPressing(
        float pressSpeed,
        float releaseSpeed)
    {
        this._pressSpeed = pressSpeed;
        this._releaseSpeed = releaseSpeed;
    }

    public void Press(float press, float deltaTime)
    {
        Value = Mathf.Lerp(Value, press, _pressSpeed * deltaTime);
    }

    public void Release(float deltaTime)
    {
        Value = Mathf.Lerp(Value, 0, _releaseSpeed * deltaTime);
        
        if(Value < sensitivity)
        {
            Value = 0;
        }
    }
}
