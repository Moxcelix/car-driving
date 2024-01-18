using UnityEngine;

public class CarMirrorManager : MonoBehaviour
{
    [SerializeField] private CarMirror[] _mirrors;

    public void SetActive(bool active)
    {
        foreach (var mirror in _mirrors)
        {
            mirror.SetActive(active);
        }
    }
}
