using System.Collections.Generic;
using UnityEngine;

public class ObservableManager : MonoBehaviour, IObservable
{
    [SerializeField] private CarMirror[] _mirrors;
    [SerializeField] private CarReflectionProbe[] _reflectionProbes;

    private readonly List<IObservable> _observables = new List<IObservable>();

    private void Awake()
    {
        _observables.AddRange(_mirrors);
        _observables.AddRange(_reflectionProbes);
    }

    public void SetActive(bool active)
    {
        foreach (var observable in _observables)
        {
            observable.SetActive(active);
        }
    }
}
