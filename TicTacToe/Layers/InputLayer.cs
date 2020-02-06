using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class InputLayer : Layer
    {
        public InputLayer(int size)
        {
            InputNeuron[] InputNeurons = new InputNeuron[size];
            for (int i = 0; i < size; i++)
            {
                InputNeurons[i] = new InputNeuron(this);
                InputNeurons[i].name = "inputNeuron " + i;
            }
            this.neurons = InputNeurons;
        }
        public override void adjustWeights()
        {
            throw new InvalidOperationException("Inputs cannot change their weight!");
        }

    }
}
