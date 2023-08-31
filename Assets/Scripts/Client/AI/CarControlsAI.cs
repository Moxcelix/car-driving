using Core.Car;
using System.Collections;
using UnityEngine;

public class CarControlsAI : IControls
{
    public float Gas { get; private set; }

    public float Break { get; private set; }

    public float SteerDelta { get; private set; }

    public bool SetDrivingMode { get; private set; }

    public bool SetParkingMode { get; private set; }

    public bool SetReverseMode { get; private set; }

    public bool SetNeutralMode { get; private set; }

    public bool EngineSwitch { get; private set; }

    public bool ParkingBreakSwitch { get; private set; }

    public bool EmergencySwitch { get; private set; }

    public bool LeftTurnSwitch { get; private set; }

    public bool RightTurnSwitch { get; private set; }

    public bool HeadLightSwitch { get; private set; }

    public IEnumerator TESTAI()
    {
        EngineSwitch = true;

        yield return new WaitForEndOfFrame();

        EngineSwitch = false;
        Break = 1;
        ParkingBreakSwitch = true;
        SteerDelta = 0;

        yield return new WaitForEndOfFrame();

        ParkingBreakSwitch = false;

        yield return new WaitForSeconds(2.0f);

        SetDrivingMode = true;

        yield return new WaitForSeconds(2.0f);

        SetDrivingMode = false;

        float t = 0.0f;

        while (t < 1.0f) 
        {
            t += 0.02f;

            SteerDelta = t;

            yield return new WaitForSeconds(0.02f);
        }

        Break = 0;
    }
}
