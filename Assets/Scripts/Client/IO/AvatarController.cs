using Core.Entity;
using Core.Car;

public class AvatarController
{
    public CarController CarController { get; private set; }
    public EntityController EntityController { get; private set; }


    public AvatarController(
        CarController carController,
        EntityController entityController)
    {
        CarController = carController;
        EntityController = entityController;
    }

    public void SetMoveAbility(bool state)
    {
        CarController.IsAvailable = state;
        EntityController.IsAvailable = state;
    }

    public void Update()
    {
        CarController.Update();
        EntityController.Update();
    }
}
