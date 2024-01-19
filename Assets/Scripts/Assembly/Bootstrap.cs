using Core.Entity;
using Core.GameManagment;
using Core.InputManagment;
using Core.Raycasting;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    // Configuration constants.
    private const float c_rayLength = 3.0f;

    // Serializable members.
    [SerializeField] private Controls _controls;
    [SerializeField] private EntityBody _playerBody;
    [SerializeField] private ClientUI _clientUI;
    [SerializeField] private ClientIO _clientIO;
    [SerializeField] private ViewSwitcher _viewSwitcher;
    [SerializeField] private InputSettings _inputSettings;
    [SerializeField] private Help _help;
    [SerializeField] private PauseMenu _pauseMenu;

    // Non-serialized members.
    private GameState _gameState;
    private AvatarController _avatarController;
    private InteractiveRaycast _interactiveRaycast;

    /// <summary>
    /// Creating and injecting main dependencies.
    /// </summary>
    private void Awake()
    {
        // Client IO data.
        var rayCaster = new Raycaster(
            _playerBody.HeadTransform, c_rayLength);

        // Game state set up.
        _gameState = new GameState();

        // User's avatar controller set up.
        _avatarController = new AvatarController(
            entityControls: _clientIO,
            carControls: _clientIO,
            avatarType: AvatarType.OBSERVED);
        _avatarController.ProvideEntityHandling(_playerBody);

        // Interactive raycasting set up.
        _interactiveRaycast =
            new InteractiveRaycast(rayCaster, _avatarController);

        // View switcher set up.
        _viewSwitcher.Initialize(_avatarController);

        // Client IO set up.
        _clientIO.Initialize(_gameState, _controls,
            _pauseMenu, _interactiveRaycast, _viewSwitcher);
        _clientUI.Initialize(_gameState, _controls,
            _interactiveRaycast);

        // Input settings set up.
        _inputSettings.Initialize(_controls);

        // Help set up.
        _help.Initialize(_controls);

        // Pause menu set up.
        _pauseMenu.Initialize(_gameState);
    }

    /// <summary>
    /// Updating states. 
    /// </summary>
    private void Update()
    {
        _clientIO.Update(Time.deltaTime);

        _interactiveRaycast.Update();
        _avatarController.Update();
        _viewSwitcher.Update();

        _avatarController.SetMoveAbility(_gameState.IsUnpause);
    }
}
