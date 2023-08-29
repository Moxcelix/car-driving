using Core.Player;
using Core.Car;
using Core.GameManagment;

public class UserController
{
    public CarController CarController { get; private set; }
    public PlayerController PlayerController { get; private set; }


    public UserController(GameState gameState,
        CarController carController,
        PlayerController playerController)
    {
        CarController = carController;
        PlayerController = playerController;
    }

    public void SetMoveAbility(bool state)
    {
        CarController.IsAvailable = state;
        PlayerController.IsAvailable = state;
    }

    public void Update()
    {
        CarController.Update();
        PlayerController.Update();
    }
}
