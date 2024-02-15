
using System;
using UnityEngine;

namespace Core.Car
{
    public abstract class Transmission : MonoBehaviour
    {
        public bool IsReverse { get; protected set; }
        public float Torque { get; protected set; }
        public float RPM { get; protected set; }
        public float Load { get; protected set; }
        public float Brake { get; protected set; }
        public int CurrentGear { get; protected set; }
        public abstract float GetRatio();
        public abstract void SetValues(float inputTorque, float inputRPM, float outputRPM);
        public abstract bool SendLiteral(string literal);

        public Action OnModeChange;
    }
}