using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class HiddenLayer : Layer
    {

        public HiddenLayer(int size)
        {
            HiddenNeuron[] InputNeurons = new HiddenNeuron[size];
            for (int i = 0; i < size; i++)
            {
                InputNeurons[i] = new HiddenNeuron(this);
                InputNeurons[i].name = "HiddenNeuron " + i;
            }
            this.neurons = InputNeurons;
        }
        public double calculateErrors()
        {
            double errorsum = 0;
            int i = 0;
            foreach (HiddenNeuron n in neurons)
            {
                n.calculateError(0);
                i++;
                errorsum += Math.Abs( n.error);
            }
            return errorsum / neurons.Length;
        }
    }
}
