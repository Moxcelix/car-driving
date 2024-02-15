using Core.InputManagment;
using TMPro;
using UnityEngine;

public class Help : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _carInstructionText;
    [SerializeField] private TextMeshProUGUI _engineInstructionText;
    [SerializeField] private TextMeshProUGUI _transmissionInstructionText;
    [SerializeField] private TextMeshProUGUI _lightingInstructionText;
    [SerializeField] private TextMeshProUGUI _startingInstructionText;
    [SerializeField] private TextMeshProUGUI _maneuveringInstructionText;
    [SerializeField] private TextMeshProUGUI _endingInstructionText;

    private Controls _controls;

    public void Initialize(Controls controls)
    {
        _controls = controls;
    }

    private void Update()
    {
        _carInstructionText.text = "\t" + GetCarInstruction() + "\n ";
        _engineInstructionText.text = "\t" + GetEngineInstruction() + "\n ";
        _transmissionInstructionText.text = "\t" + GetTransmissionInstruction() + "\n ";
        _lightingInstructionText.text = "\t" + GetLightingInstruction() + "\n ";
        _startingInstructionText.text = "\t" + GetStartingInstruction() + "\n ";
        _maneuveringInstructionText.text = "\t" + GetManeuveringInstruction() + "\n ";
        _endingInstructionText.text = "\t" + GetEndingInstruction() + "\n ";
    }

    private string GetEndingInstruction()
    {
        return
            $"Перед концом движения рекомендуется включить правый указатель поворота " +
            $"[{_controls["right_blinker"]}]." +
            $"Чтобы закончить движение, нужно полностью остановить автомобиль " +
            $"с помощью педали тормоза [{_controls["brake"]}]. Затем, не " +
            $"отпуская тормоз, перевести коробку передач в положение P " +
            $"[{_controls["parking"]}]. Pекомендуется поднять стояночный " +
            $"тормоз [{_controls["parking_brake"]}]. После чего можно отпустить педаль " +
            $"тормоза. В случае необходимости можно заглушить двигатель " +
            $"[{_controls["ignition"]}].";
    }

    private string GetLightingInstruction()
    {
        return
            $"Габаритные огни и фары ближнего света включаются автоматически с " +
            $"запуском двигателя [{_controls["ignition"]}]. Чтобы включить " +
            $"дальний свет фар, нужно нажать [{_controls["head_light"]}]. " +
            $"Чтобы включить левый указатель " +
            $"поворота, нужно нажать [{_controls["left_blinker"]}], чтобы включить правый – " +
            $"[{_controls["right_blinker"]}]. Чтобы включить аварийную сигнализацию, " +
            $"используется клавиша [{_controls["emergency"]}].";
    }

    private string GetStartingInstruction()
    {
        return 
            $"Перед началом движения рекомендуется включить левый указатель поворота " +
            $"[{_controls["left_blinker"]}]. Нужно опустить ручной тормоз, нажав " +
            $"[{_controls["parking_brake"]}], затем нужно выжать педаль тормоза " +
            $"[{_controls["brake"]}], перевести коробку передач в положение D " +
            $"[{_controls["driving"]}], отпустить педаль тормоза. После того, как " +
            $"автомобиль тронется с места, отключить указатель поворота " +
            $"[{_controls["left_blinker"]}]. Для ускорения следует нажать педаль газа " +
            $"[{_controls["gas"]}].";
    }

    private string GetManeuveringInstruction()
    {
        return 
            $"Для управлением скоростью и направлением автомобиля используются " +
            $"педали тормоза и газа и руль, соответсвенно.  Чтобы тормозить, нужно " +
            $"нажать и удерживать [{_controls["brake"]}]. Чтобы набрать скорость, " +
            $"нужно нажать и удерживать [{_controls["gas"]}]. Для вращения руля " +
            $"вправо следует удерживать [{_controls["right_steer"]}], для врещения " +
            $"руля влево - [{_controls["left_steer"]}]. Чтобы нажать педаль сильнее, " +
            $"следует удерживать [{_controls["power"]}].";
    }

    private string GetCarInstruction()
    {
        return 
            $"Чтобы воспользоваться автомобилем, подойти к выбранной машине, " +
            $"навести камеру на водительскую дверь. Нажать клавишу " +
            $"[{_controls["interact"]}], чтобы открыть дверь. После открытия двери " +
            $"навести камеру на сиденье, нажать [{_controls["interact"]}], " +
            $"после чего игровой персонаж сядет за руль. Далее рекомендуется " +
            $"закрыть за собой дверь, так же с помощью [{_controls["interact"]}].";
    }

    private string GetEngineInstruction()
    {
        return
            $"Чтобы завести или заглушить двигатель, " +
            $"нужно нажать [{_controls["ignition"]}].";
    }

    private string GetTransmissionInstruction()
    {
        return
            $"По умолчанию коробка передач находится в положении P (Паркинг). " +
            $"Переключение селектора передачи выполняется строго при полной " +
            $"остановке и выжатой педалью тормоза (удерживая [{_controls["brake"]}])." +
            $"Всего доступно четыре режима: \n\tP (нажать [{_controls["parking"]}]) " +
            $"- Паркинг, движение автомобиля " +
            $"заблокировано; \n\tR (нажать [{_controls["reverse"]}]) - Задний ход, " +
            $"автомобиль движется назад;" +
            $"\n\tN (нажать [{_controls["neutral"]}]) - Нейтраль, движение автомобиля " +
            $"не контролируется; \n\t" +
            $"D (нажать [{_controls["driving"]}]) - Движение вперед, основной режим " +
            $"коробки при вождении.";
    }
}
