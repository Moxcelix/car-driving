using Core.Grabing;
using Core.Raycasting;

public class GrabbingRaycast
{
    private readonly Raycaster _raycaster;
    private readonly Carrier _carrier;

    private Item _item;

    public string Hint { get; private set; }
    public bool IsFocused { get; private set; }

    public GrabbingRaycast(Raycaster raycaster, Carrier carrier)
    {
        _raycaster = raycaster;
        _carrier = carrier;

        _item = null;
    }

    public void Update()
    {
        _item = CastItem();

        Hint = _item?.Hint ?? string.Empty;

        IsFocused = _item is not null;
    }

    public void TryGrab()
    {
        if (_carrier.Item != null)
        {
            _carrier.Drop();

            return;
        }

        if (_item is null)
        {
            return;
        }

        _carrier.Take(_item);
    }

    private Item CastItem()
    {
        var raycastHit = _raycaster.CheckHit<Item>();

        if (raycastHit?.IsTaken ?? false)
        {
            return null;
        }

        return raycastHit;
    }
}
