using UnityEngine;

public static class MouseController
{
    private static bool state = false;

    public static void SetVisibility(bool state)
    {
        MouseController.state = state;
        Cursor.lockState = state ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = state;
    }

    public static void SwitchState()
    {
        SetVisibility(!state);
    }
}
