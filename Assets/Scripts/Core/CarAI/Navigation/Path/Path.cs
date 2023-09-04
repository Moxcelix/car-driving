namespace Core.CarAI.Navigation
{
    public class Path
    {
        private readonly Node _startNode;
        private readonly Node _endNode;

        public Path(Node startNode, Node endNode)
        {
            _startNode = startNode;
            _endNode = endNode;
        }
    }
}