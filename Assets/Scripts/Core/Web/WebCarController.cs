namespace Core.Web
{
    using Core.Car;
    public class WebCarController
    {
        private readonly Car _car;

        public WebCarController(Car car)
        {
            _car = car;
        }

        public void Update()
        {
            SendData();
            ReceiveData();
        }

        private void SendData()
        {
            // Send
        }

        private void ReceiveData()
        {
            // Receive
        }
    }
}
