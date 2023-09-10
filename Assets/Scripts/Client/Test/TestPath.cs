using Core.CarAI;
using Core.CarAI.Navigation;
using UnityEngine;

public class TestPath : MonoBehaviour
{
    [SerializeField] private Node _startNode;
    [SerializeField] private Node _endNode;

    private PathFinder _pathFinder;

    private void Awake()
    {
        _pathFinder = new PathFinder();
    }

    private void Start()
    {
        var path = _pathFinder.CreatePath(_startNode, _endNode);

        Debug.Log(path);
    }
}
