using Core.Car;
using Core.GameManagment;
using Core.InputManagment;
using Core.Player;
using Core.Raycasting;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    // Configuration constants.
    private const float c_rayLength = 3.0f;

    // Serializable members.
    [SerializeField] private Controls _controls;
    [SerializeField] private PlayerBody _playerBody;
    [SerializeField] private ClientUI _clientUI;
    [SerializeField] private ClientIO _clientIO;
    [SerializeField] private ViewSwitcher _viewSwitcher;
    [SerializeField] private InputSettings _inputSettings;
    [SerializeField] private Help _help;
    [SerializeField] private PauseMenu _pauseMenu;

    // Non-serialized members.
    private GameState _gameState;
    private UserController _userController;
    private InteractiveRaycast _interactiveRaycast;

    /// <summary>
    /// Creating and injecting main dependencies.
    /// </summary>
    private void Awake()
    {
        // User Controller data.
        var carController = new CarController(_clientIO);
        var playerController = new PlayerController(_clientIO);
        playerController.SetPlayerBody(_playerBody);

        // Client IO data.
        var rayCaster = new Raycaster(
            _playerBody.HeadTransform, c_rayLength);

        // Game state set up.
        _gameState = new GameState();

        // User controller set up.
        _userController = new UserController( 
            _gameState, carController, playerController);
        _userController.SetMoveAbility(true);

        // Interactive raycasting set up.
        _interactiveRaycast = 
            new InteractiveRaycast(rayCaster, _userController);

        // View switcher set up.
        _viewSwitcher.Initialize(_userController);

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
        _interactiveRaycast.Update();
        _clientIO.Update();
        _userController.Update();
        _viewSwitcher.Update();
    }
}
