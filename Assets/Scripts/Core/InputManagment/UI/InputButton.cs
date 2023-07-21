using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.InputManagment
{
    public class InputButton : MonoBehaviour
    {
        private KeyValue _keyValue;
        private bool _isCatching = false;

        [SerializeField] private TextMeshProUGUI _paramName;
        [SerializeField] private TextMeshProUGUI _keyName;
        [SerializeField] private Image _image;
        [SerializeField] private GameObject _cancelButton;

        [SerializeField] private Color _common;
        [SerializeField] private Color _selected;

        private static bool isBusy = false;

        public KeyValue KeyValue
        {
            get
            {
                return _keyValue;
            }
            set
            {
                if(value == _keyValue) 
                {
                    return;
                }

                _keyValue = value;

                UpdateState();
            }
        }

        private void Update()
        {
            _image.color = _isCatching ? _selected : _common;

            _cancelButton.SetActive(_isCatching);
        }

        private void UpdateState()
        {
            _paramName.text = KeyValue.Name.ToString();
            _keyName.text = KeyValue.KeyCode.ToString();
        }

        public void StartCatching()
        {
            if (isBusy)
            {
                return;
            }

            _isCatching = true;
            isBusy = true;
        }

        public void CancelCatching()
        {
            _isCatching = false;
            isBusy = false;
        }

        private void OnGUI()
        {
            if (!_isCatching)
            {
                return;
            }

            if (Event.current is not null && Event.current.isKey)
            {
                KeyValue.KeyCode = Event.current.keyCode;

                UpdateState();

                _isCatching = false;
                isBusy = false;
            }
        }
    }
}