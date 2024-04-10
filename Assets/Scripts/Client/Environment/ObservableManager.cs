using UnityEngine;

public class ObservableManager : MonoBehaviour, IObservable
{
    [SerializeField] private CarMirror[] _mirrors;
    [SerializeField] private CarReflectionProbe[] _reflectionProbe;

    public void SetActive(bool active)
    {
        foreach (var mirror in _mirrors)
        {
            mirror.SetActive(active);
        }

        foreach (var reflectionProbe in _reflectionProbe)
        {  
            reflectionProbe.SetActive(active); 
        }
    }
}
