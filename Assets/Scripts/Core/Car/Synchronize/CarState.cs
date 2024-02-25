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
        public (int x, int y) TransmissionSelectorPosition { get; set; }
        public BlinkerState BlinkerState { get; set; }
        public bool Emergency { get; set; }
        public float TurnAmount { get; set; }
        public (float x, float y, float z) LeftFrontWheelPosition { get; set; }
        public (float x, float y, float z) LeftFrontWheelRotation { get; set; }
        public (float x, float y, float z) LeftRearWheelPosition { get; set; }
        public (float x, float y, float z) LeftRearWheelRotation { get; set; }
        public (float x, float y, float z) RightFrontWheelPosition { get; set; }
        public (float x, float y, float z) RightFrontWheelRotation { get; set; }
        public (float x, float y, float z) RightRearWheelPosition { get; set; }
        public (float x, float y, float z) RightRearWheelRotation { get; set; }
    }
}