using UnityEngine;

namespace Core.Cameras
{
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(AudioListener))]
    public class MovableCamera : MonoBehaviour
    {
        [SerializeField] private bool _isMovable = true;

        protected Camera _camera;
        private AudioListener _listener;

        public bool IsMovable => _isMovable;

        private void Awake()
        {
            Initialize();
        }

        public void SetMovable(bool state)
        {
            _isMovable = state;
        }

        public void SetActive(bool state)
        {
            if (_camera == null)
            {
                Initialize();
            }
            _camera.enabled = state;
            _listener.enabled = state;
        }

        private void Initialize()
        {
            _camera = GetComponent<Camera>();
            _listener = GetComponent<AudioListener>();
        }
    }
}