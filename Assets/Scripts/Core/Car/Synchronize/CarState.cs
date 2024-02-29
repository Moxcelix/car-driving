using Newtonsoft.Json;
using UnityEngine;

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

        public static CarState Lerp(CarState a, CarState b, float t)
        {
            (float x, float y, float z) LerpTupple(
                (float x, float y, float z) a, 
                (float x, float y, float z) b, float t) =>(
                Mathf.Lerp(a.x, b.x, t),  
                Mathf.Lerp(a.y, b.y, t), 
                Mathf.Lerp(a.z, b.z, t));

            (float x, float y, float z) LerpTuppleRotation(
                (float x, float y, float z) a,
                (float x, float y, float z) b, float t)
            {
                var rotationA = new Quaternion();
                var rotationB = new Quaternion();
                rotationA.eulerAngles = new Vector3(a.x, a.y, a.z);
                rotationB.eulerAngles = new Vector3(b.x, b.y, b.z);

                var rotation = Quaternion.Lerp(rotationA, rotationB, t);
                var eulerAngles = rotation.eulerAngles;

                return (eulerAngles.x, eulerAngles.y, eulerAngles.z);
            }



            return new CarState()
            {
                RPM = Mathf.Lerp(a.RPM, b.RPM, t),
                Speed = Mathf.Lerp(a.Speed, b.Speed, t),
                Brake = Mathf.Lerp(a.Brake, b.Brake, t),
                Gas =  Mathf.Lerp(a.Gas, b.Gas, t),
                Clutch = Mathf.Lerp(a.Clutch, b.Clutch, t),
                EngineState = b.EngineState,
                ParkingBrake = b.ParkingBrake,
                HighLight = b.HighLight,
                TransmissionSelectorPosition = b.TransmissionSelectorPosition,
                BlinkerState = b.BlinkerState,
                Emergency = b.Emergency,
                TurnAmount = Mathf.Lerp(a.TurnAmount, b.TurnAmount, t),
                LeftFrontWheelPosition = LerpTupple(a.LeftFrontWheelPosition, b.LeftFrontWheelPosition, t),
                RightFrontWheelPosition = LerpTupple(a.RightFrontWheelPosition, b.RightFrontWheelPosition, t),
                LeftRearWheelPosition = LerpTupple(a.LeftRearWheelPosition, b.LeftRearWheelPosition, t),
                RightRearWheelPosition = LerpTupple(a.RightRearWheelPosition, b.RightRearWheelPosition, t),
                LeftFrontWheelRotation = LerpTuppleRotation(a.LeftFrontWheelRotation, b.LeftFrontWheelRotation, t),
                RightFrontWheelRotation = LerpTuppleRotation(a.RightFrontWheelRotation, b.RightFrontWheelRotation, t),
                LeftRearWheelRotation = LerpTuppleRotation(a.LeftRearWheelRotation, b.LeftRearWheelRotation, t),
                RightRearWheelRotation = LerpTuppleRotation(a.RightRearWheelRotation, b.RightRearWheelRotation, t),
                BodyPosition = LerpTupple(a.BodyPosition, b.BodyPosition, t),
                BodyRotation = LerpTupple(a.BodyRotation, b.BodyRotation, t),
            };
        }

    }
}