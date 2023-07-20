using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CarID))]
public class CarIdApplier : MonoBehaviour
{
    [Serializable]
    public class Plate
    {
        [SerializeField] private Text _number;
        [SerializeField] private Text _region;

        public void SetID(CarID carID)
        {
            _number.text = carID.Number;
            _region.text = carID.Region;
        }
    }

    [SerializeField] private Plate[] _plates;

    private CarID _carID;

    private void Awake()
    {
        _carID = GetComponent<CarID>();
    }

    private void Start()
    {
        foreach (var plate in _plates)
        {
            plate.SetID(_carID);
        }
    }
}
