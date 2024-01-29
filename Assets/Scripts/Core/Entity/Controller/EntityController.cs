namespace Core.Entity
{
    public class EntityController
    {
        private readonly IControls _controls;

        private readonly EntityBody _entityBody;

        private bool _closed = false;

        public EntityBody EntityBody => _entityBody;

        public bool IsAvailable { get; set; }

        public EntityController(IControls controls, EntityBody entityBody)
        {
            this._controls = controls;
            this._entityBody = entityBody;

            _controls.Leave = Leave;

            IsAvailable = true;
        }

        public void Close()
        {
            _controls.Leave = null;

            _closed = true;
        }

        public void Update()
        {
            if (_entityBody == null)
            {
                return;
            }

            if (IsAvailable)
            {
                _entityBody.Move(_controls.MoveSide, _controls.MoveForward);
            }

            _entityBody.IsRunning = IsAvailable && _controls.IsRunning;
            _entityBody.IsJumping = IsAvailable && _controls.IsJumping;

            var rotationDelta = new UnityEngine.Vector3(
                IsAvailable ? _controls.RotationDeltaX : 0.0f,
                IsAvailable ? _controls.RotationDeltaY : 0.0f
            );

            _entityBody.UpdateView(rotationDelta);
        }

        private void Leave()
        {
            if (IsAvailable)
            {
                _entityBody.IsSitting = false;
            }
        }

        ~EntityController()
        {
            if (_closed)
            {
                return;
            }

            Close();
        }

    }
}
