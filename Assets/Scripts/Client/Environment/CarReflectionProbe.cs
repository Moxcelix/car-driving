using UnityEngine;

public class CarReflectionProbe : MonoBehaviour
{
    [SerializeField] private ReflectionProbe _reflectionProbe;

    public void SetActive(bool active)
    {
        _reflectionProbe.refreshMode = active ? 
            UnityEngine.Rendering.ReflectionProbeRefreshMode.EveryFrame : 
            UnityEngine.Rendering.ReflectionProbeRefreshMode.OnAwake;
    }
}
