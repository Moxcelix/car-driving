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
        return $"����� ��������������� �����������, ������� � ��������� ������, " +
            $"������� ������ �� ������������ �����. ������ ������� " +
            $"[{_controls["interact"]}], ����� ������� �����. ����� �������� ����� " +
            $"������� ������ �� �������, ������ [{_controls["interact"]}], " +
            $"����� ���� ������� �������� ����� �� ����. ����� ������������� " +
            $"������� �� ����� �����, ��� �� � ������� [{_controls["interact"]}].";
    }

    private string GetEngineInstruction()
    {
        return
            $"����� ������� ��� ��������� ���������, " +
            $"����� ������ [{_controls["ignition"]}]";
    }

    private string GetTransmissionInstruction()
    {
        return
            $"�� ��������� ������� ������� ��������� � ��������� P (�������). " +
            $"������������ ��������� �������� ����������� ������ ��� ������ " +
            $"��������� � ������� �������� (��������� [{_controls["break"]}])." +
            $"����� �������� ������ ������: \n\tP (������ [{_controls["parking"]}]) " +
            $"- �������, �������� ���������� " +
            $"�������������; \n\tR (������ [{_controls["reverse"]}]) - ������ ���, " +
            $"���������� �������� �����;" +
            $"\n\tN (������ [{_controls["neutral"]}]) - ��������, �������� ���������� " +
            $"�� ��������������; \n\t" +
            $"D (������ [{_controls["driving"]}]) - �������� ������, �������� ����� " +
            $"������� ��� ��������.";
    }

    public string GetInstruction()
    {
        return
            $"����� ��������������� �����������, ����� ��������� " +
            $"��������� �������� ��������. ������� � ��������� ������, " +
            $"������� ������ �� ������������ �����. ������ ������� " +
            $"[{_controls["interact"]}], ����� ������� �����. ����� �������� ����� " +
            $"������� ������ �� �������, ������ [{_controls["interact"]}], " +
            $"����� ���� ������� �������� ����� �� ����. ����� ������������� " +
            $"������� �� ����� �����, ��� �� � ������� [{_controls["interact"]}] " +
            $"����� ������ [{_controls["ignition"]}], ����� ������� " +
            $"��������� ����������. ����� ������ ��������, ����� �������� " +
            $"������ ������, ����� [{_controls["parking_break"]}], ������ ������, ����� �� " +
            $"[{_controls["break"]}], � ��������� �������� ������� ������� � ��������� D, " +
            $"����� ������� [{_controls["driving"]}]. ����� ������ ��������� ������, n������ " +
            $"������. ����� �������� ����������, ���� ������ ������ ����, " +
            $"��� ����� ������������  [{_controls["gas"]}]. ��� �� ������ ������ " +
            $"�������, ����� ���������� ������� [{_controls["power"]}]. ��� �������� " +
            $"���� ������ � ����� ������������ ������� " +
            $"[{_controls["right_steer"]}] � [{_controls["left_steer"]}] " +
            $"��������������. ����� ����� �����, ��������� ��������� ������ ��������� " +
            $"������������� ��������, ����� ��������� �������� ������� " +
            $"������� � ��������� R, ����� [{_controls["reverse"]}]. ��� ����, ����� " +
            $"���������� �������� ���������� � ����� �� ����, ����� ��������� " +
            $"n������������, ��������� �������� ������� ������� � ��������� P, " +
            $"����� �� [{_controls["parking"]}], ����� ������������� �������� " +
            $"������ ���������� ������, ����� �� [{_controls["parking_break"]}], ����� ���� " +
            $"������� ������ �� �����, ������ [{_controls["interact"]}], " +
            $"����� �������� ����� ������ [{_controls["leave"]}], ����� ������ " +
            $"� �������. �� ����� �������� ��� �� �������� ������������� " +
            $"�������� ��������� ���������. ����� �������� ����� ��������� " +
            $"��������, ����� ������ [{_controls["left_blinker"]}], ����� �������� ������ � " +
            $"[{_controls["right_blinker"]}]. ����� �������� ��������� ������������, " +
            $"������������ ������� [{_controls["emergency"]}], ����� �������� ������� ���� " +
            $"��� � ������� [{_controls["head_light"]}].";
    }
}
