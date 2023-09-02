using Core.Car;
using Core.CarAI;
using System.Collections;
using UnityEngine;

public class CarControlsAI : IControls
{
    private TargetFollow _targetFollow;

    public float Gas { get; private set; }

    public float Break { get; private set; }

    public float SteerDelta { get; private set; }

    public bool ParkingBreakSwitch { get; private set; }

    public bool EmergencySwitch { get; private set; }

    public bool HighLightSwitch { get; private set; }

    public bool EngineState { get; private set; }

    public TransmissionMode TransmissionMode { get; private set; }

    public BlinkerState BlinkerState { get; private set; }

    public CarControlsAI(TargetFollow targetFollow)
    {
        _targetFollow = targetFollow;
    }

    public IEnumerator TESTAI()
    {
        EngineState = true;
        Break = 1;
        ParkingBreakSwitch = false;
        SteerDelta = 0;

        yield return new WaitForSeconds(2.0f);

        TransmissionMode = TransmissionMode.DRIVING;

        yield return new WaitForSeconds(2.0f);

        Break = 0;

        while (true)
        {
            SteerDelta = _targetFollow.TurnAmount;
            Gas = Mathf.Abs(_targetFollow.ForwardAmount);

            if(_targetFollow.ForwardAmount < 0 && TransmissionMode == TransmissionMode.DRIVING)
            {
                Break = 1;

                yield return new WaitForSeconds(2.0f);

                TransmissionMode = TransmissionMode.REVERSE;

                Break = 0;
            }

            if (_targetFollow.ForwardAmount >= 0 && TransmissionMode != TransmissionMode.DRIVING)
            {
                Break = 1;

                yield return new WaitForSeconds(2.0f);

                TransmissionMode = TransmissionMode.DRIVING;

                Break = 0;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
