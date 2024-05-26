using UnityEngine;

public class AnglePlane : MonoBehaviour
{
    [SerializeField] private Transform _plane;

    public void SetAngle(float angle)
    {
        var angles = _plane.eulerAngles;
        angles.x = angle;
        _plane.eulerAngles = angles;
    }
}
