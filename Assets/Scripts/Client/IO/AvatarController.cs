using Core.Entity;
using Core.Car;

public class AvatarController
{
    private readonly Core.Entity.IControls _entityControls;
    private readonly Core.Car.IControls _carControls;

    public CarController CarController { get; private set; }

    public EntityController EntityController { get; private set; }


    public AvatarController(
        Core.Entity.IControls entityControls,
        Core.Car.IControls carControls)
    {
        _carControls = carControls;
        _entityControls = entityControls;
    }

    public void SetMoveAbility(bool state)
    {
        if(CarController is not null)
        {
            CarController.IsAvailable = state;
        }
        
        if(EntityController is not null)
        {
            EntityController.IsAvailable = state;
        }
    }

    public void Update()
    {
        CarController?.Update();
        EntityController?.Update();
    }

    public void ProvideCarHandling(Car car)
    {
        CarController = new CarController(_carControls, car);
    }

    public void ProvideEntityHandling(EntityBody entity)
    {
        EntityController = new EntityController(_entityControls, entity);
    }

    public void DepriveCarHandling()
    {
        CarController?.Close();

        CarController = null;
    }

    public void DepriveEntityHandling()
    {
        EntityController?.Close();

        EntityController = null;
    }
}
