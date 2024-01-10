using UnityEngine;

public class JoystickPress
{
    public delegate void OnPressDelegate();

    public event OnPressDelegate OnPress;

    private readonly string _key;
    private readonly bool _isPositive = true;

    private bool _isPressed = false;

    public JoystickPress(string key, bool isPositive)
    {
        _key = key;
        _isPositive = isPositive;
    }

    public void Update()
    {
        if (Input.GetAxis(_key) * (_isPositive ? 1.0f : -1.0f) > 0.5f)
        {
            if (_isPressed)
            {
                OnPress?.Invoke();

                _isPressed = true;
            }
        }
        else
        {
            _isPressed = false;
        }
    }
}
