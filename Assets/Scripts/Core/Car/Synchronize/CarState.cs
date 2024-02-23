using UnityEngine;

namespace Core.Car
{
    public class CarState
    {
        public float RPM { get; set; }
        public float Speed { get; set; }
        public float Brake { get; set; }
        public float Gas { get; set; }
        public float Clutch { get; set; }
        public bool EngineState { get; set; }
        public bool ParkingBrake { get; set; }
        public bool HighLight { get; set; }
        public Vector2Int TransmissionSelectorPosition { get; set; }
        public BlinkerState BlinkerState { get; set; }
        public bool Emergency { get; set; }
        public float TurnAmount { get; set; }
        public Transform LeftFrontWheel { get; set; }
        public Transform LeftRearWheel { get; set; }
        public Transform RightFrontWheel { get; set; }
        public Transform RightRearWheel { get; set; }
        public bool[] DoorStates { get; set; }
    }
}