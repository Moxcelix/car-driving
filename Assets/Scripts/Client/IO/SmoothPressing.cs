using UnityEngine;

[System.Serializable]
public class SmoothPressing
{
    const float sensitivity = 0.01f;

    [SerializeField] private float _pressSpeed = 0.1f;
    [SerializeField] private float _releaseSpeed = 0.1f;

    public float Value { get; private set; }

    public void Press(float press, float deltaTime)
    {
        Value = Mathf.Lerp(Value, press, _pressSpeed * deltaTime);
    }

    public void Release(float deltaTime)
    {
        Value -= _releaseSpeed * deltaTime;

        if (Value < sensitivity)
        {
            Value = 0;
        }
    }
}
