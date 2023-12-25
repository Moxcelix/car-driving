namespace Core.Carsharing
{
    using Core.Car;
    using Newtonsoft.Json;
    using System;

    public class Telemetry
    {
        private readonly Car _car;
        private readonly int _id;

        private const string on = "on";
        private const string off = "off";

        public Telemetry(Car car, int id)
        {
            _car = car;
            _id = id;
        }

        public string GetData()
        {
            TelemetryData telemetryData = new()
            {
                Timedate = DateTime.UtcNow,
                CarId = _id,
                Data = new TelemetryDataDetails
                {
                    LeftFrontDoorStatus = GetDoorStatus(_car.Doors[0]),
                    LeftRearDoorStatus = GetDoorStatus(_car.Doors[1]),
                    RightFrontDoorStatus = GetDoorStatus(_car.Doors[2]),
                    RightRearDoorStatus = GetDoorStatus(_car.Doors[3]),
                    TrunkStatus = GetDoorStatus(_car.Doors[4]),
                    HoodStatus = GetDoorStatus(_car.Doors[5]),
                    Geoposition = GetGeoposition(_car),
                    Speed = GetSpeed(_car),
                    ImmobilizerStatus = GetImmobilizerStatus(_car),
                    CentralLockingStatus = GetCentralLockingStatus(_car)
                }
            };

            return JsonConvert.SerializeObject(telemetryData, Formatting.Indented);
        }

        private string GetDoorStatus(Door door)
        {
            return door.State == IOpenable.OpenState.CLOSED ? "closed" : "opened";
        }

        private string GetGeoposition(Car car)
        {
            return
                $"{car.gameObject.transform.position.x} " +
                $"{car.gameObject.transform.position.z}";
        }

        private float GetSpeed(Car car)
        {
            return car.GetSpeed() * 3.6f;
        }

        private string GetImmobilizerStatus(Car car)
        {
            return car.Immobilizer.IsActive ? on : off;
        }

        private string GetCentralLockingStatus(Car car)
        {
            return car.CentralLocking.Locked ? on : off;
        }
    }
}