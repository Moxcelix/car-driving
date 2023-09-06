using System;

namespace Core.Neural
{
    public class Layer
    {
        private readonly int _numberOfInputs;
        private readonly int _numberOfOuputs;
        private readonly float _learningRate;

        private readonly float[] _outputs;
        private readonly float[,] _weights;
        private readonly float[,] _weightsDelta;
        private readonly float[] _gamma;
        private readonly float[] _error;

        private float[] _inputs;

        public float[] Outputs => _outputs;
        public float[] Gamma => _gamma;
        public float[,] Weights => _weights;

        public Layer(
            int numberOfInputs,
            int numberOfOuputs,
            float learningRate = 0.033f)
        {
            this._numberOfInputs = numberOfInputs;
            this._numberOfOuputs = numberOfOuputs;
            this._learningRate = learningRate;

            _outputs = new float[numberOfOuputs];
            _inputs = new float[numberOfInputs];
            _weights = new float[numberOfOuputs, numberOfInputs];
            _weightsDelta = new float[numberOfOuputs, numberOfInputs];
            _gamma = new float[numberOfOuputs];
            _error = new float[numberOfOuputs];
        }

        public void InitilizeWeights(IWeightProvider weightProvider)
        {
            for (int i = 0; i < _numberOfOuputs; i++)
            {
                for (int j = 0; j < _numberOfInputs; j++)
                {
                    _weights[i, j] = (float)weightProvider.GetWeight(i, j);
                }
            }
        }

        public float[] FeedForward(float[] inputs)
        {
            _inputs = inputs;

            for (int i = 0; i < _numberOfOuputs; i++)
            {
                _outputs[i] = 0;

                for (int j = 0; j < _numberOfInputs; j++)
                {
                    _outputs[i] += inputs[j] * _weights[i, j];
                }

                _outputs[i] = (float)Math.Tanh(_outputs[i]);
            }

            return _outputs;
        }

        public float TanHDer(float value)
        {
            return 1.0f - (value * value);
        }

        public void BackPropOutput(float[] expected)
        {
            for (int i = 0; i < _numberOfOuputs; i++)
            {
                _error[i] = _outputs[i] - expected[i];
            }

            for (int i = 0; i < _numberOfOuputs; i++)
            {
                _gamma[i] = _error[i] * TanHDer(_outputs[i]);
            }

            for (int i = 0; i < _numberOfOuputs; i++)
            {
                for (int j = 0; j < _numberOfInputs; j++)
                {
                    _weightsDelta[i, j] = _gamma[i] * _inputs[j];
                }
            }
        }

        public void BackPropHidden(float[] gammaForward, float[,] weightsFoward)
        {
            for (int i = 0; i < _numberOfOuputs; i++)
            {
                _gamma[i] = 0;

                for (int j = 0; j < gammaForward.Length; j++)
                {
                    _gamma[i] += gammaForward[j] * weightsFoward[j, i];
                }

                _gamma[i] *= TanHDer(_outputs[i]);
            }

            for (int i = 0; i < _numberOfOuputs; i++)
            {
                for (int j = 0; j < _numberOfInputs; j++)
                {
                    _weightsDelta[i, j] = _gamma[i] * _inputs[j];
                }
            }
        }

        public void UpdateWeights()
        {
            for (int i = 0; i < _numberOfOuputs; i++)
            {
                for (int j = 0; j < _numberOfInputs; j++)
                {
                    _weights[i, j] -= _weightsDelta[i, j] * _learningRate;
                }
            }
        }
    }
}