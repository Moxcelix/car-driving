using Core.GameManagment;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private readonly int _helpPageNumber = 1;

    private GameState _gameState;

    [SerializeField] private MenuSwitcher _menuSwitcher;

    public void Initialize(GameState gameState)
    {
        _gameState = gameState;
    }

    public void Back()
    {
        _gameState.Unpause();
    }

    public void OpenHelp()
    {
        _menuSwitcher.OpenPage(_helpPageNumber);
    }
}
