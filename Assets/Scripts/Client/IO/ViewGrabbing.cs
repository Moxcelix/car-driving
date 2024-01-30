using Core.Grabing;
using UnityEngine;

public class ViewGrabbing
{
    private const float indent = 0.0f;
    private const float strartIndent = 0.5f;

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
        if(_carrier.Item == null) return;

        var position = _transform.position + _transform.forward * (_rayLength - indent);

        if (Physics.Raycast(_transform.position + _transform.forward * strartIndent,
                _transform.forward, out RaycastHit hit, (_rayLength - strartIndent)))
        {
            if(hit.distance < strartIndent)
            {
                position = _transform.position + _transform.forward * strartIndent;
            }
            else
            {
                position = hit.point - indent * _transform.forward;
            }
        }

        _carrier.Grab(position);
    }
}
