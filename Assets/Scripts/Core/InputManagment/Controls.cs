using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Core.InputManagment
{
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

                if (!query.Any())
                {
                    throw new System.ArgumentException("No registered key value.");
                }

                return query.First().KeyCode;
            }
        }
    }
}