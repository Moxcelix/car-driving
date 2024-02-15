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

        private SelectorPosition _selector;

        public void SwitchUp()
        {
            ChangeValue(Vector2Int.up);
        }

        public void SwitchDown()
        {
            ChangeValue(Vector2Int.down);
        }

        public void SwitchRight()
        {
            ChangeValue(Vector2Int.right);
        }

        public void SwitchLeft()
        {
            ChangeValue(Vector2Int.left);
        }

        private void ChangeValue(Vector2Int delta)
        {
            var position = GetPosition(_selector.Position + delta);

            if (position == null)
            {
                return;
            }

            _selector = position;

            _transmission.SendLiteral(_selector.Literal);
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
