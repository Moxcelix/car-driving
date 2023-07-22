using Core.GameManagment;
using Core.InputManagment;
using TMPro;
using UnityEngine;

public class ClientUI : MonoBehaviour
{
    [SerializeField] private GameObject _cursor;
    [SerializeField] private GameObject _pauseBackground;
    [SerializeField] private TextMeshProUGUI _hintKey;
    [SerializeField] private TextMeshProUGUI _hintText;

    private InteractiveRaycast _interactiveRaycast;
    private GameState _gameState;
    private Controls _controls;
    // TODO: fix this.
    private readonly string _key = "interact";

    public void Initialize(GameState gameState,
        Controls controls,
        InteractiveRaycast interactiveRaycast)
    {
        this._gameState = gameState;
        this._interactiveRaycast = interactiveRaycast;
        this._controls = controls;
    }

    private void Update()
    {
        _cursor.SetActive(_interactiveRaycast.IsFocused
            && _gameState.IsUnpause);
        _pauseBackground.SetActive(_gameState.IsPause);

        _hintKey.text = $"[{_controls[_key]}]";
        _hintText.text = _interactiveRaycast.Hint;

        MouseController.SetVisibility(_gameState.IsPause);
    }
}
