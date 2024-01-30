using Core.Grabing;
using UnityEngine;

public class ViewGrabbing
{
    private const float indent = 0.5f;

    private readonly Carrier _carrier;
    private readonly Transform _transform;
    private readonly float _rayLength;

    public ViewGrabbing(Carrier carrier, Transform transform, float rayLength)
    {
        _carrier = carrier;
        _transform = transform;
        _rayLength = rayLength;
    }

    public void Update()
    {
        var position = _transform.forward * (_rayLength - indent);

        if (Physics.Raycast(_transform.position,
                _transform.forward, out RaycastHit hit, _rayLength))
        {
            position = hit.point - indent * _transform.forward;
        }

        _carrier.Grab(position);
    }
}
