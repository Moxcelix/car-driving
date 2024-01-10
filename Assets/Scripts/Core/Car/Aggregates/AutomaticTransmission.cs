using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Car
{
    public class AutomaticTransmission : MonoBehaviour, ITransmission
    {
        float ITransmission.Torque => throw new System.NotImplementedException();

        float ITransmission.RPM => throw new System.NotImplementedException();

        float ITransmission.Load => throw new System.NotImplementedException();

        float ITransmission.Brake => throw new System.NotImplementedException();

        int ITransmission.CurrentGear => throw new System.NotImplementedException();

        void ITransmission.SwitchDown()
        {
            throw new System.NotImplementedException();
        }

        void ITransmission.SwitchLeft()
        {
            throw new System.NotImplementedException();
        }

        void ITransmission.SwitchRight()
        {
            throw new System.NotImplementedException();
        }

        void ITransmission.SwitchUp()
        {
            throw new System.NotImplementedException();
        }
    }
}