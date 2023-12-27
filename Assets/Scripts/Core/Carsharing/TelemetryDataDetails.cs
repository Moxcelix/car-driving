using Newtonsoft.Json;

namespace Core.Carsharing
{
    public class TelemetryDataDetails
    {
        [JsonProperty("left_front_door_status")]
        public string LeftFrontDoorStatus { get; set; }

        [JsonProperty("left_rear_door_status")]
        public string LeftRearDoorStatus { get; set; }

        [JsonProperty("right_front_door_status")]
        public string RightFrontDoorStatus { get; set; }

        [JsonProperty("right_rear_door_status")]
        public string RightRearDoorStatus { get; set; }

        [JsonProperty("trunk_status")]
        public string TrunkStatus { get; set; }

        [JsonProperty("hood_status")]
        public string HoodStatus { get; set; }

        [JsonProperty("geoposition")]
        public string Geoposition { get; set; }

        [JsonProperty("speed")]
        public float Speed { get; set; }

        [JsonProperty("immobilizer_status")]
        public string ImmobilizerStatus { get; set; }

        [JsonProperty("central_locking_status")]
        public string CentralLockingStatus { get; set; }

        [JsonProperty("parking_brake_status")]
        public string ParkingBrakeStatus { get; set; }
    }
}