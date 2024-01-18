using UnityEngine;

public class CarMirrorManager : MonoBehaviour
{
    [SerializeField] private RenderTexture _leftMirrorTexture;
    [SerializeField] private RenderTexture _rightMirrorTexture;

    [SerializeField] private Camera _leftMirrorCamera;
    [SerializeField] private Camera _rightMirrorCamera;
}
