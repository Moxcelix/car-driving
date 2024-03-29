using Core.Raycasting;
using Core.Grabing;

public class InteractiveRaycast
{
    private readonly AvatarController _userController;
    private readonly Raycaster _raycaster;

    private IInteractive _interactive;

    public string Hint { get; private set; }
    public bool IsFocused { get; private set; }

    public InteractiveRaycast(Raycaster raycaster,
        AvatarController userController, Carrier carrier)
    {
        _userController = userController;
        _raycaster = raycaster;

        _interactive = null;
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
