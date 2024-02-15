using UnityEngine;

namespace Core.Car
{
    [System.Serializable]
    public class SelectorPosition
    {
        [SerializeField] private Vector2Int _position;
        [SerializeField] private string _literal;
        [SerializeField] private GameObject _selectorObject;

        public Vector2Int Position => _position;
        public string Literal => _literal;
        public GameObject SelectorObject => _selectorObject;
    }

    public class TransmissionSelector : MonoBehaviour
    {
        [SerializeField] private Transmission _transmission;
        [SerializeField] private SelectorPosition[] _selectorPositions;

        private Vector2Int _currentPosition = Vector2Int.zero;

        public void SwitchUp()
        {

        }

        public void SwitchDown()
        {

        }

        public void SwitchRight()
        {

        }

        public void SwitchLeft()
        {

        }

        private void ChangeValue(Vector2Int delta)
        {

        }

        private SelectorPosition GetPosition(Vector2Int position)
        {
            foreach (var pos in _selectorPositions)
            {
                if (pos.Position == position)
                {
                    return pos;
                }
            }

            return null;
        }
    }
}
