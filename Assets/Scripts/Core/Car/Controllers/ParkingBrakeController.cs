using UnityEngine;

namespace Core.Car
{
    public class ParkingBrakeController : Controller
    {
        [SerializeField] private ParkingBrake _parkingBrake;

        public override bool Check()
        {
            return _parkingBrake.State != ParkingBrakeState.LOWERED;
        }
    }
}