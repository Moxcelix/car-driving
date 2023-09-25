using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Core.Neural
{
    public class WeightSave : IWeightProvider
    {
        private float[,] _weights;
        public double GetWeight(int i, int j)
        {
            throw new System.NotImplementedException();
        }

        public void Load(string path)
        {
            var stream = new FileStream(path, FileMode.Open);
            var binaryFormatter = new BinaryFormatter();

            _weights = (float[,])binaryFormatter.Deserialize(stream);

            stream.Close();
        }

        public void Save(string path, float[,] weights)
        {
            var stream = new FileStream(path, FileMode.OpenOrCreate);
            var binaryFormatter = new BinaryFormatter();

            binaryFormatter.Serialize(stream, weights);

            stream.Close();
        }
    }
}
