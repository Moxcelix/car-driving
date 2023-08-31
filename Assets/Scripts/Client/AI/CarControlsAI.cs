using Core.Car;
using System.Collections;
using UnityEngine;

public class CarControlsAI : IControls
{
    public float Gas { get; private set; }

    public float Break { get; private set; }

    public float SteerDelta { get; private set; }

    public bool ParkingBreakSwitch { get; private set; }

    public bool EmergencySwitch { get; private set; }

    public bool HighLightSwitch { get; private set; }

    public bool EngineState { get; private set; }

    public TransmissionMode TransmissionMode { get; private set; }

    public BlinkerState BlinkerState { get; private set; }

    public IEnumerator TESTAI()
    {
        EngineState = true;
        Break = 1;
        ParkingBreakSwitch = false;
        SteerDelta = 0;

        yield return new WaitForSeconds(2.0f);

        TransmissionMode = TransmissionMode.DRIVING;

        yield return new WaitForSeconds(2.0f);

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
