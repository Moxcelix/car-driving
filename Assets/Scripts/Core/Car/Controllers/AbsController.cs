using UnityEngine;

namespace Core.Car
{
    public class AbsController : Controller
    {
        [SerializeField] private Car _car;

        public override bool Check()
        {
            return _car.Engine.Starter.IsStarting || _car.BrakeSystem.ABS > 0;
        }
    }
}