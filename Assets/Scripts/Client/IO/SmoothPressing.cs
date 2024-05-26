using UnityEngine;

[System.Serializable]
public class SmoothPressing
{
    const float sensitivity = 0.01f;

    [SerializeField] private float _pressSpeed = 0.1f;
    [SerializeField] private float _releaseSpeed = 0.1f;
    [SerializeField] private AnimationCurve _pressCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] private AnimationCurve _releaseCurve = AnimationCurve.Linear(0, 0, 1, 1);

    private float _progress;

    public float Value { get; private set; }

    public void Press(float press, float deltaTime)
    {
        _progress = Mathf.Lerp(_progress, press, _pressSpeed * deltaTime);

        Value = _pressCurve.Evaluate(_progress);
    }

    public void Release(float press, float deltaTime)
    {
        _progress -= _releaseSpeed * deltaTime;

        if (_progress < press + sensitivity)
        {
            _progress = press;
        }

        Value = _releaseCurve.Evaluate(_progress);
    }
}
