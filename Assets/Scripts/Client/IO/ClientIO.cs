using Core.Car;
using Core.GameManagment;
using Core.InputManagment;
using System;
using TMPro;
using UnityEngine;

[Serializable]
public class ClientIO :
    Core.Car.IControls,
    Core.Entity.IControls
{
    private readonly string _forwardKey = "forward";
    private readonly string _backKey = "back";
    private readonly string _rightKey = "right";
    private readonly string _leftKey = "left";
    private readonly string _jumpKey = "jump";
    private readonly string _runKey = "run";
    private readonly string _leaveKey = "leave";
    private readonly string _gasKey = "gas";
    private readonly string _brakeKey = "brake";
    private readonly string _setDrivingModeKey = "driving";
    private readonly string _setParkingModeKey = "parking";
    private readonly string _setReverseModeKey = "reverse";
    private readonly string _setNeutralModeKey = "neutral";
    private readonly string _steerRightKey = "right_steer";
    private readonly string _steerLeftKey = "left_steer";
    private readonly string _engineSwitchKey = "ignition";
    private readonly string _parkingBrakeKey = "parking_brake";
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

    private readonly SmoothPressing _gasSmoothPressing = new(0.5f, 0.5f);
    private readonly SmoothPressing _brakeSmoothPressing = new(1f, 1.5f);

    private readonly float _gasMiddleValue = 0.5f;
    private readonly float _brakeMiddleValue = 0.6f;

    private GameState _gameState;
    private PauseMenu _pauseMenu;
    private Controls _controls;
    private InteractiveRaycast _interactiveRaycast;
    private ViewSwitcher _viewSwitcher;

    private bool _engineState = false;
    private bool _emergencyState = false;
    private bool _highLightState = false;
    private bool _parkingBrakeState = true;
    private BlinkerState _blinkerState = BlinkerState.None;

    public event IControls.ToogleSwitchDelegate EngineSwitch;
    public event IControls.BlinkerStateSwitchDelegate BlinkerStateSwitch;
    public event IControls.TransmissionModeSwitchDelegate TransmissionModeSwitch;
    public event IControls.ToogleSwitchDelegate HighLightSwitch;
    public event IControls.ToogleSwitchDelegate EmergencySwitch;
    public event IControls.ToogleSwitchDelegate ParkingBrakeSwitch;

    // Car controls.
    public float Gas { get; private set; } = 0;

    public float Brake { get; private set; } = 0;

    public float SteerDelta { get; private set; } = 0;

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

    public void Update(float deltaTime)
    {
        HandleCarInput(deltaTime);

        HandleViewSwitching();
        HandlePauseSwitch();
        HandlePlayerInput();
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

    private void HandleCarInput(float deltaTime)
    {
        var fullPush = Input.GetKey(_controls[_addPowerKey]);

        if (Input.GetKey(_controls[_gasKey]))
        {
            _gasSmoothPressing.Press(fullPush ? 1.0f : _gasMiddleValue, deltaTime);
        }
        else
        {
            _gasSmoothPressing.Release(deltaTime);
        }

        if (Input.GetKey(_controls[_brakeKey]))
        {
            _brakeSmoothPressing.Press(fullPush ? 1.0f : _brakeMiddleValue, deltaTime);
        }
        else
        {
            _brakeSmoothPressing.Release(deltaTime);
        }

        Gas = _gasSmoothPressing.Value;
        Brake = _brakeSmoothPressing.Value;

        var rightSteering = Input.GetKey(_controls[_steerRightKey]) ?
            deltaTime : 0.0f;
        var leftSteering = Input.GetKey(_controls[_steerLeftKey]) ?
            deltaTime : 0.0f;
        SteerDelta = rightSteering - leftSteering;

        if (Input.GetKeyDown(_controls[_setDrivingModeKey]))
        {
            TransmissionModeSwitch?.Invoke(TransmissionMode.DRIVING);
        }

        if (Input.GetKeyDown(_controls[_setReverseModeKey]))
        {
            TransmissionModeSwitch?.Invoke(TransmissionMode.REVERSE);
        }

        if (Input.GetKeyDown(_controls[_setParkingModeKey]))
        {
            TransmissionModeSwitch?.Invoke(TransmissionMode.PARKING);
        }

        if (Input.GetKeyDown(_controls[_setNeutralModeKey]))
        {
            TransmissionModeSwitch?.Invoke(TransmissionMode.NEUTRAL);
        }

        // Toggle-sus
        void Toggle(BlinkerState state)
        {
            _blinkerState =
                _blinkerState == state ?
                BlinkerState.None : state;

            BlinkerStateSwitch?.Invoke(_blinkerState);
        }

        if (Input.GetKeyDown(_controls[_leftTurnKey]))
        {
            Toggle(BlinkerState.Left);
        }

        if (Input.GetKeyDown(_controls[_rightTurnKey]))
        {
            Toggle(BlinkerState.Rigth);
        }

        if (Input.GetKeyDown(_controls[_emergencyKey]))
        {
            _emergencyState = !_emergencyState;

            EmergencySwitch?.Invoke(_emergencyState);
        }

        if (Input.GetKeyDown(_controls[_headLightKey]))
        {
            _highLightState = !_highLightState;

            HighLightSwitch?.Invoke(_highLightState);
        }

        if (Input.GetKeyDown(_controls[_engineSwitchKey]))
        {
            _engineState = !_engineState;

            EngineSwitch?.Invoke(_engineState);
        }

        if (Input.GetKeyDown(_controls[_parkingBrakeKey]))
        {
            _parkingBrakeState = !_parkingBrakeState;

            ParkingBrakeSwitch?.Invoke(_parkingBrakeState);
        }
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
