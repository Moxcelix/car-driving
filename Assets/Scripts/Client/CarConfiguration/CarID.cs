using UnityEngine;

public class CarID : MonoBehaviour
{
    [SerializeField] private string _number;
    [SerializeField] private string _region;

    public string ID => _number + _region;
    public string Number => _number;
    public string Region => _region;
}
