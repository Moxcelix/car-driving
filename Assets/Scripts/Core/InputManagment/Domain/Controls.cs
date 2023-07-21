using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Core.InputManagment
{
    public class Controls : MonoBehaviour
    {
        [SerializeField] private List<KeyValue> _keyValues;

        public List<KeyValue> KeyValues => _keyValues;

        public KeyCode this[string name]
        {
            get
            {
                var query = from entry in _keyValues
                            where entry.Name == name
                            select entry;

                if (!query.Any())
                {
                    throw new System.ArgumentException($"No registered key value: {name}.");
                }

                return query.First().KeyCode;
            }

            set
            {
                var query = from entry in _keyValues
                            where entry.Name == name
                            select entry;

                if (!query.Any())
                {
                    throw new System.ArgumentException($"No registered key value: {name}.");
                }

                query.First().KeyCode = value;
            }
        }
    }
}