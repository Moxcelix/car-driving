namespace Core.Entity
{
    public class EntityController
    {
        private readonly IControls _controls;

        private EntityBody _entityBody;

        public bool IsAvailable { get; set; }

        public EntityBody EntityBody => _entityBody;

        public EntityController(IControls controls)
        {
            this._controls = controls;

            IsAvailable = true;
        }

        public void SetEntityBody(EntityBody playerBody)
        {
            this._entityBody = playerBody;
        }

        public void Update()
        {
            if(_entityBody == null) return;

            var forward = IsAvailable && _controls.MoveForward ?
                Movement.POSITIVE :
                Movement.NONE;

            var back = IsAvailable && _controls.MoveBack ?
                Movement.NEGATIVE :
                Movement.NONE;

            var right = IsAvailable && _controls.MoveRight ?
                Movement.POSITIVE :
                Movement.NONE;

            var left = IsAvailable && _controls.MoveLeft ?
                Movement.NEGATIVE :
                Movement.NONE;

            var horizontal = (Movement)((int)right + (int)left);
            var vertical = (Movement)((int)forward + (int)back);

            _entityBody.Move(horizontal, vertical);

            _entityBody.IsRunning = IsAvailable && _controls.IsRunning;
            _entityBody.IsJumping = IsAvailable && _controls.IsJumping;

            var rotationDelta = new UnityEngine.Vector3(
                IsAvailable ? _controls.RotationDeltaX : 0.0f,
                IsAvailable ? _controls.RotationDeltaY : 0.0f
            );

            _entityBody.UpdateView(rotationDelta);

            if (IsAvailable && _controls.Leave)
            {
                _entityBody.IsSitting = false;
            }
        }
    }
}
