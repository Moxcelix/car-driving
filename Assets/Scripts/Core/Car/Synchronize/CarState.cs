using Newtonsoft.Json;

namespace Core.Car
{
    public class CarState
    {

        [JsonProperty("rpm")]
        public float RPM { get; set; }

        [JsonProperty("spepd")]
        public float Speed { get; set; }

        [JsonProperty("brake")]
        public float Brake { get; set; }

        [JsonProperty("gas")]
        public float Gas { get; set; }

        [JsonProperty("clutch")]
        public float Clutch { get; set; }

        [JsonProperty("engine")]
        public bool EngineState { get; set; }

        [JsonProperty("parking")]
        public bool ParkingBrake { get; set; }

        [JsonProperty("head_light")]
        public bool HighLight { get; set; }

        [JsonProperty("selector")]
        public (int x, int y) TransmissionSelectorPosition { get; set; }

        [JsonProperty("blinker")]
        public BlinkerState BlinkerState { get; set; }

        [JsonProperty("emergency")]
        public bool Emergency { get; set; }

        [JsonProperty("turn")]
        public float TurnAmount { get; set; }

        [JsonProperty("lfw_pos")]
        public (float x, float y, float z) LeftFrontWheelPosition { get; set; }

        [JsonProperty("lfw_rot")]
        public (float x, float y, float z) LeftFrontWheelRotation { get; set; }

        [JsonProperty("lrw_pos")]
        public (float x, float y, float z) LeftRearWheelPosition { get; set; }

        [JsonProperty("lrw_rot")]
        public (float x, float y, float z) LeftRearWheelRotation { get; set; }

        [JsonProperty("rfw_pos")]
        public (float x, float y, float z) RightFrontWheelPosition { get; set; }

        [JsonProperty("rfw_rot")]
        public (float x, float y, float z) RightFrontWheelRotation { get; set; }

        [JsonProperty("rrw_pos")]
        public (float x, float y, float z) RightRearWheelPosition { get; set; }

        [JsonProperty("rrw_rot")]
        public (float x, float y, float z) RightRearWheelRotation { get; set; }

        [JsonProperty("pos")]
        public (float x, float y, float z) BodyPosition { get; set; }

        [JsonProperty("rot")]
        public (float x, float y, float z) BodyRotation { get; set; }

    }
}