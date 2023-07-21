using UnityEngine;

namespace Core.InputManagment
{
    public class InputSettings : MonoBehaviour
    {
        [SerializeField] private InputButton _inputButtonPrefab;
        [SerializeField] private Transform _contentParent;

        private Controls _controls;

        public void Initialize(Controls controls)
        {
            _controls = controls;

            foreach (var keyValue in _controls.KeyValues)
            {
                var button = Instantiate(_inputButtonPrefab, _contentParent);

                button.gameObject.transform.localScale = Vector3.one;
                button.KeyValue = keyValue;
            }
        }


    }
}