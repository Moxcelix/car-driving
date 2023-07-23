using Core.InputManagment;
using TMPro;
using UnityEngine;

public class Help : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _carInstructionText;
    [SerializeField] private TextMeshProUGUI _engineInstructionText;
    [SerializeField] private TextMeshProUGUI _transmissionInstructionText;

    private Controls _controls;

    public void Initialize(Controls controls)
    {
        _controls = controls;
    }

    private void Update()
    {
        _carInstructionText.text = GetCarInstruction() + "\n ";
        _engineInstructionText.text = GetEngineInstruction() + "\n ";
        _transmissionInstructionText.text = GetTransmissionInstruction() + "\n ";
    }

    private string GetCarInstruction()
    {
        return $"Чтобы воспользоваться автомобилем, подойти к выбранной машине, " +
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
            $"нужно нажать [{_controls["ignition"]}]";
    }

    private string GetTransmissionInstruction()
    {
        return
            $"По умолчанию коробка передач находится в положении P (Паркинг). " +
            $"Переключение селектора передачи выполняется строго при полной " +
            $"остановке и выжатым тормозом (удерживая [{_controls["break"]}])." +
            $"Всего доступно четыре режима: \n\tP (нажать [{_controls["parking"]}]) " +
            $"- Паркинг, движение автомобиля " +
            $"заблокировано; \n\tR (нажать [{_controls["reverse"]}]) - Задний ход, " +
            $"автомобиль движется назад;" +
            $"\n\tN (нажать [{_controls["neutral"]}]) - Нейтраль, движение автомобиля " +
            $"не контролируется; \n\t" +
            $"D (нажать [{_controls["driving"]}]) - Движение вперед, основной режим " +
            $"коробки при вождении.";
    }

    public string GetInstruction()
    {
        return
            $"Чтобы воспользоваться автомобилем, нужно выполнить " +
            $"следующий алгоритм действий. Подойти к выбранной машине, " +
            $"навести камеру на водительскую дверь. Нажать клавишу " +
            $"[{_controls["interact"]}], чтобы открыть дверь. После открытия двери " +
            $"навести камеру на сиденье, нажать [{_controls["interact"]}], " +
            $"после чего игровой персонаж сядет за руль. Далее рекомендуется " +
            $"закрыть за собой дверь, так же с помощью [{_controls["interact"]}] " +
            $"Затем нажать [{_controls["ignition"]}], чтобы завести " +
            $"двигатель автомобиля. Чтобы начать движение, нужно опустить " +
            $"ручной тормоз, нажав [{_controls["parking_break"]}], выжать тормоз, нажав на " +
            $"[{_controls["break"]}], и перевести селектор коробки передач в положение D, " +
            $"нажав клавишу [{_controls["driving"]}]. Затем плавно отпустить тормоз, nмашина " +
            $"поедет. Чтобы ускорить автомобиль, надо нажать педаль газа, " +
            $"для этого используется  [{_controls["gas"]}]. Что бы нажать педали " +
            $"сильнее, нужно удерживать клавишу [{_controls["power"]}]. Для вращения " +
            $"руля вправо и влево используются клавиши " +
            $"[{_controls["right_steer"]}] и [{_controls["left_steer"]}] " +
            $"соответственно. Чтобы сдать назад, требуется выполнить полную остановку " +
            $"транспортного средства, затем перевести селектор коробки " +
            $"передач в положение R, нажав [{_controls["reverse"]}]. Для того, чтобы " +
            $"прекратить движение автомобиля и выйти из него, нужно полностью " +
            $"nостановиться, перевести селектор коробки передач в положение P, " +
            $"нажав на [{_controls["parking"]}], затем рекомендуется затянуть " +
            $"ручной стояночный тормоз, нажав на [{_controls["parking_break"]}], после чего " +
            $"навести камеру на дверь, нажать [{_controls["interact"]}], " +
            $"после открытия двери нажать [{_controls["leave"]}], чтобы встать " +
            $"с сиденья. Во время движения так же доступно использование " +
            $"внешними световыми приборами. Чтобы включить левый указатель " +
            $"поворота, нужно нажать [{_controls["left_blinker"]}], чтобы включить правый – " +
            $"[{_controls["right_blinker"]}]. Чтобы включить аварийную сигнализацию, " +
            $"используется клавиша [{_controls["emergency"]}], чтобы включить дальний свет " +
            $"фар – клавиша [{_controls["head_light"]}].";
    }
}
