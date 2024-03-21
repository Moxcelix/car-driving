using UnityEngine;

namespace Core.Car
{
    public class AbsController : Controller
    {
        [SerializeField] private Car _car;

        public override bool Check()
        {
            return _car.BrakeSystem.ABS > 0;
        }
    }
}