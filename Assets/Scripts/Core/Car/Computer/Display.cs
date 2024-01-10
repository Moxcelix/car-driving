using UnityEngine;
using UnityEngine.UI;

namespace Core.Car
{
    public class Display : MonoBehaviour
    {

        [SerializeField] private Car _car;
        [SerializeField] private Text _speedText;
        [SerializeField] private Text _gearText;
        [SerializeField] private Text _modeText;
        [SerializeField] private Text _mileageText;

        [SerializeField] private int _instantaneousConsumptionDevisions;
        [SerializeField] private Text _instantaneousConsumptionText;

        private Computer _computer;

        private void Start()
        {
            _computer = _car.Computer;
        }

        private void Update()
        {
            if (_car.Engine.Starter.State == EngineState.STOPED)
            {
                _speedText.text = string.Empty;
                _gearText.text = string.Empty;
                _modeText.text = string.Empty;
                _mileageText.text = string.Empty;
                _instantaneousConsumptionText.text = string.Empty;

                return;
            }

            _gearText.text =
                _computer.TransmissionMode == AutomaticTransmissionMode.DRIVING ||
                _computer.TransmissionMode == AutomaticTransmissionMode.MANUAL ?
                _computer.Gear.ToString() : string.Empty;

            _mileageText.text = ((int)_computer.Mileage).ToString() + " KM";
            _modeText.text = _computer.TransmissionMode.ToString();
            _speedText.text = _computer.Speed.ToString() + " KM/H";
            _instantaneousConsumptionText.text =
                new string('=',
                (int)(_computer.Consumption * _instantaneousConsumptionDevisions));
        }
    }
}