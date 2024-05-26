using UnityEngine;
using UnityEngine.UI;

public class AnglePlaneUI : MonoBehaviour
{
    [SerializeField] private AnglePlane _plane;
    [SerializeField] private Slider _slider;

    private void Update()
    {
        _plane.SetAngle(90.0f * (-_slider.value));
    }
}
