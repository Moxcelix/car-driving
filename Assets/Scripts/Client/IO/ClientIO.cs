using Core.GameManagment;
using Core.InputManagment;
using System;
using UnityEngine;

[Serializable]
public class ClientIO :
    Core.Car.IControls,
    Core.Player.IControls
{
    private readonly string _forwardKey = "forward";
    private readonly string _backKey = "back";
    private readonly string _rightKey = "right";
    private readonly string _leftKey = "left";
    private readonly string _jumpKey = "jump";
    private readonly string _runKey = "run";
    private readonly string _leaveKey = "leave";
    private readonly string _gasKey = "gas";
    private readonly string _breakKey = "break";
    private readonly string _setDrivingModeKey = "driving";
    private readonly string _setParkingModeKey = "parking";
    private readonly string _setReverseModeKey = "reverse";
    private readonly string _setNeutralModeKey = "neutral";
    private readonly string _steerRightKey = "right_steer";
    private readonly string _steerLeftKey = "left_steer";
    private readonly string _engineSwitchKey = "ignition";
    private readonly string _parkingBreakKey = "parking_break";
    private readonly string _addPowerKey = "power";
    private readonly string _leftTurnKey = "left_blinker";
    private readonly string _rightTurnKey = "right_blinker";
    private readonly string _emergencyKey = "emergency";
    private readonly string _headLightKey = "head_light";
    private readonly string _interactKey = "interact";
    private readonly string _switchViewKey = "next_view";

    [SerializeField] private float _mouseSensitivity = 2;
    private readonly KeyCode _pauseKey = KeyCode.Escape;
    private readonly KeyCode _helpKey = KeyCode.F1;

    private readonly SmoothPressing gasSmoothPressing = new(0.5f, 0.5f, 0.5f);
    private readonly SmoothPressing breakSmoothPressing = new(1f, 1.5f, 0.6f);

    private GameState _gameState;
    private PauseMenu _pauseMenu;
    private Controls _controls;
    private InteractiveRaycast _interactiveRaycast;
    private ViewSwitcher _viewSwitcher;

    // Car controls.
    public float Gas { get; private set; }
    public float Break { get; private set; }
    public float SteerDelta { get; private set; }
    public bool SetDrivingMode { get; private set; }
    public bool SetParkingMode { get; private set; }
    public bool SetReverseMode { get; private set; }
    public bool SetNeutralMode { get; private set; }
    public bool EngineSwitch { get; private set; }
    public bool ParkingBreakSwitch { get; private set; }
    public bool EmergencySwitch { get; private set; }
    public bool LeftTurnSwitch { get; private set; }
    public bool RightTurnSwitch { get; private set; }
    public bool HeadLightSwitch { get; private set; }

    // Character controls.
    public float RotationDeltaX { get; private set; }
    public float RotationDeltaY { get; private set; }
    public bool MoveForward { get; private set; }
    public bool MoveBack { get; private set; }
    public bool MoveRight { get; private set; }
    public bool MoveLeft { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsJumping { get; private set; }
    public bool Leave { get; private set; }

    public void Initialize(GameState gameState,
        Controls controls, PauseMenu pauseMenu,
        InteractiveRaycast interactiveRaycast,
        ViewSwitcher viewSwitcher)
    {
        this._gameState = gameState;
        this._controls = controls;
        this._pauseMenu = pauseMenu;
        this._interactiveRaycast = interactiveRaycast;
        this._viewSwitcher = viewSwitcher;

        MouseController.SetVisibility(false);
    }

    public void Update()
    {
        HandleViewSwitching();
        HandlePauseSwitch();
        HandlePlayerInput();
        HandleCarInput();
        HandleInteract();
        HandleHelpOpen();
    }

    private void HandlePlayerInput()
    {
        RotationDeltaX = Input.GetAxis("Mouse X") * _mouseSensitivity;
        RotationDeltaY = Input.GetAxis("Mouse Y") * _mouseSensitivity;

        MoveForward = Input.GetKey(_controls[_forwardKey]);
        MoveBack = Input.GetKey(_controls[_backKey]);
        MoveRight = Input.GetKey(_controls[_rightKey]);
        MoveLeft = Input.GetKey(_controls[_leftKey]);
        IsRunning = Input.GetKey(_controls[_runKey]);
        IsJumping = Input.GetKey(_controls[_jumpKey]);
        Leave = Input.GetKeyDown(_controls[_leaveKey]);
    }

    private void HandleCarInput()
    {
        if (Input.GetKey(_controls[_gasKey]))
        {
            gasSmoothPressing.Press();
        }
        else
        {
            gasSmoothPressing.Release();
        }

        if (Input.GetKey(_controls[_breakKey]))
        {
            breakSmoothPressing.Press();
        }
        else
        {
            breakSmoothPressing.Release();
        }

        gasSmoothPressing.FullPush =
            breakSmoothPressing.FullPush =
            Input.GetKey(_controls[_addPowerKey]);

        Gas = gasSmoothPressing.Value;
        Break = breakSmoothPressing.Value;

        var rightSteering = Input.GetKey(_controls[_steerRightKey]) ?
            Time.deltaTime : 0.0f;
        var leftSteering = Input.GetKey(_controls[_steerLeftKey]) ?
            Time.deltaTime : 0.0f;
        SteerDelta = rightSteering - leftSteering;

        SetDrivingMode = Input.GetKeyDown(_controls[_setDrivingModeKey]);
        SetReverseMode = Input.GetKeyDown(_controls[_setReverseModeKey]);
        SetParkingMode = Input.GetKeyDown(_controls[_setParkingModeKey]);
        SetNeutralMode = Input.GetKeyDown(_controls[_setNeutralModeKey]);
        LeftTurnSwitch = Input.GetKeyDown(_controls[_leftTurnKey]);
        RightTurnSwitch = Input.GetKeyDown(_controls[_rightTurnKey]);
        EmergencySwitch = Input.GetKeyDown(_controls[_emergencyKey]);
        HeadLightSwitch = Input.GetKeyDown(_controls[_headLightKey]);
        EngineSwitch = Input.GetKeyDown(_controls[_engineSwitchKey]);
        ParkingBreakSwitch = Input.GetKeyDown(_controls[_parkingBreakKey]);
    }

    private void HandlePauseSwitch()
    {
        if (Input.GetKeyDown(_pauseKey))
        {
            _gameState.SwitchPauseState();
        }
    }

    private void HandleHelpOpen()
    {
        if (Input.GetKeyDown(_helpKey))
        {
            _gameState.Pause();
            _pauseMenu.OpenHelp();
        }
    }

    private void HandleInteract()
    {
        if (_gameState.IsPause)
        {
            return;
        }

        if (Input.GetKeyDown(_controls[_interactKey]) ||
            Input.GetMouseButtonDown(0))
        {
            _interactiveRaycast.TryInteract();
        }
    }

    private void HandleViewSwitching()
    {
        _viewSwitcher.Enable(_gameState.IsUnpause);

        if (Input.GetKeyDown(_controls[_switchViewKey]))
        {
            _viewSwitcher.Switch();
        }
    }
}
