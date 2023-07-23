using Core.InputManagment;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class HelpCase
{
    [SerializeField] private TextMeshProUGUI _hint;

    private Controls _controls;
    private string _text;

    public void Initialize(Controls controls)
    {
        _controls = controls;
    }

    public void Update()
    {
        _hint.text = _text;
    }
}
