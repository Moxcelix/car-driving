using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Controls 
{
    [SerializeField] private List<KeyValue> _keyValues;

    public KeyCode this[string name]
    {
        get
        {
            var query = from entry in _keyValues
                        where entry.Name == name
                        select entry;

            return query.First().KeyCode;
        }
    }
}