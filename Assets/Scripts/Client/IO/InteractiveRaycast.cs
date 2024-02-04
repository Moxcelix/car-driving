using Core.Raycasting;
using Core.Grabing;

public class InteractiveRaycast
{
    private readonly AvatarController _userController;
    private readonly Raycaster _raycaster;
    private readonly Carrier _carrier;

    private IInteractive _interactive;
    private Item _item;

    public string Hint { get; private set; }
    public bool IsFocused { get; private set; }

    public InteractiveRaycast(Raycaster raycaster,
        AvatarController userController, Carrier carrier)
    {
        _userController = userController;
        _raycaster = raycaster;
        _carrier = carrier;

        _interactive = null;
        _item = null;
    }

    public void Update()
    {
        _interactive = CastInteractive();

        Hint = _interactive?.Hint ?? string.Empty;

        IsFocused = _interactive is not null;
    }

    public void TryInteract()
    {
        if (_interactive is null)
        {
            return;
        }

        _interactive.Interact(_userController);
    }

    public void TryGrab()
    {
        if(_carrier.Item != null)
        {
            _carrier.Drop();

            return;
        }

        if(_item is null)
        {
            return;
        }

        _carrier.Take(_item);
    }

    private IInteractive CastInteractive()
    {
        var raycastHit = _raycaster.CheckHit<IInteractive>();

        if (!raycastHit?.IsInteractable(_userController) ?? false)
        {
            return null;
        }

        return raycastHit;
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
