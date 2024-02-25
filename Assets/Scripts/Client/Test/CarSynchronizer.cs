using UnityEngine;
using Core.Car;

public class CarSynchronizer : MonoBehaviour
{
    [SerializeField] private Car _parentCar;
    [SerializeField] private Car _childCar;

    private void Start()
    {
        _childCar.Syncable = true;
        _parentCar.Syncable = false;
    }

    private void Update()
    {
        _childCar.Synchronize(_parentCar.GetSyncState());   
        _childCar.transform.SetPositionAndRotation(
            _parentCar.transform.position + Vector3.right * 20.0f, 
            _parentCar.transform.rotation);
    }
}
