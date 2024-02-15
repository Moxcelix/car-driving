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
            $"����� ������ �������� ������������� �������� ������ ��������� �������� " +
            $"[{_controls["right_blinker"]}]." +
            $"����� ��������� ��������, ����� ��������� ���������� ���������� " +
            $"� ������� ������ ������� [{_controls["brake"]}]. �����, �� " +
            $"�������� ������, ��������� ������� ������� � ��������� P " +
            $"[{_controls["parking"]}]. P������������ ������� ���������� " +
            $"������ [{_controls["parking_brake"]}]. ����� ���� ����� ��������� ������ " +
            $"�������. � ������ ������������� ����� ��������� ��������� " +
            $"[{_controls["ignition"]}].";
    }

    private string GetLightingInstruction()
    {
        return
            $"���������� ���� � ���� �������� ����� ���������� ������������� � " +
            $"�������� ��������� [{_controls["ignition"]}]. ����� �������� " +
            $"������� ���� ���, ����� ������ [{_controls["head_light"]}]. " +
            $"����� �������� ����� ��������� " +
            $"��������, ����� ������ [{_controls["left_blinker"]}], ����� �������� ������ � " +
            $"[{_controls["right_blinker"]}]. ����� �������� ��������� ������������, " +
            $"������������ ������� [{_controls["emergency"]}].";
    }

    private string GetStartingInstruction()
    {
        return 
            $"����� ������� �������� ������������� �������� ����� ��������� �������� " +
            $"[{_controls["left_blinker"]}]. ����� �������� ������ ������, ����� " +
            $"[{_controls["parking_brake"]}], ����� ����� ������ ������ ������� " +
            $"[{_controls["brake"]}], ��������� ������� ������� � ��������� D " +
            $"[{_controls["driving"]}], ��������� ������ �������. ����� ����, ��� " +
            $"���������� �������� � �����, ��������� ��������� �������� " +
            $"[{_controls["left_blinker"]}]. ��� ��������� ������� ������ ������ ���� " +
            $"[{_controls["gas"]}].";
    }

    private string GetManeuveringInstruction()
    {
        return 
            $"��� ����������� ��������� � ������������ ���������� ������������ " +
            $"������ ������� � ���� � ����, �������������.  ����� ���������, ����� " +
            $"������ � ���������� [{_controls["brake"]}]. ����� ������� ��������, " +
            $"����� ������ � ���������� [{_controls["gas"]}]. ��� �������� ���� " +
            $"������ ������� ���������� [{_controls["right_steer"]}], ��� �������� " +
            $"���� ����� - [{_controls["left_steer"]}]. ����� ������ ������ �������, " +
            $"������� ���������� [{_controls["power"]}].";
    }

    private string GetCarInstruction()
    {
        return 
            $"����� ��������������� �����������, ������� � ��������� ������, " +
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
            $"����� ������ [{_controls["ignition"]}].";
    }

    private string GetTransmissionInstruction()
    {
        return
            $"�� ��������� ������� ������� ��������� � ��������� P (�������). " +
            $"������������ ��������� �������� ����������� ������ ��� ������ " +
            $"��������� � ������� ������� ������� (��������� [{_controls["brake"]}])." +
            $"����� �������� ������ ������: \n\tP (������ [{_controls["parking"]}]) " +
            $"- �������, �������� ���������� " +
            $"�������������; \n\tR (������ [{_controls["reverse"]}]) - ������ ���, " +
            $"���������� �������� �����;" +
            $"\n\tN (������ [{_controls["neutral"]}]) - ��������, �������� ���������� " +
            $"�� ��������������; \n\t" +
            $"D (������ [{_controls["driving"]}]) - �������� ������, �������� ����� " +
            $"������� ��� ��������.";
    }
}
