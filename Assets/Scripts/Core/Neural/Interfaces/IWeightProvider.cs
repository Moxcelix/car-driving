namespace Core.Neural
{
    public interface IWeightProvider
    {
        double GetWeight(int i, int j);
    }
}