public interface IInteractive
{
    public string Hint { get; }

    public bool IsInteractable (AvatarController userController);
    public void Interact(AvatarController userController);
}
