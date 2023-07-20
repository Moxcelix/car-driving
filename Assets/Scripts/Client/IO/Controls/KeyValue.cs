using System;
using UnityEngine;

[Serializable]
public class KeyValue
{
    [SerializeField] private string _name = "?";
    [SerializeField] private KeyCode _keyCode = KeyCode.None;

    public string Name => _name;
    public KeyCode KeyCode => _keyCode;
}
