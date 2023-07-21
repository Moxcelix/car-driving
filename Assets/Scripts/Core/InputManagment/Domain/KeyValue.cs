using System;
using UnityEngine;

namespace Core.InputManagment
{
    [Serializable]
    public class KeyValue
    {
        [SerializeField] private string _name = "?";
        [SerializeField] private KeyCode _keyCode = KeyCode.None;

        public string Name
        {
            get => _name;
            set => _name = value;
        } 

        public KeyCode KeyCode
        {
            get => _keyCode;
            set => _keyCode = value;
        } 
    }
}
