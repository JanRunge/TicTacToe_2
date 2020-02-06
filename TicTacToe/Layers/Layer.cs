using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    abstract class Layer
    {
        public Neuron[] neurons;
        public Function ActivationFunction = new Sigmoid();

        public void fire()
        {
            foreach (Neuron n in neurons)
            {
                n.fire();
            }
        }
        public void connectToInputLayer(Layer l)
        {
            //connect every neuron in layer1 to every neuron in layer 2
            foreach (Neuron myNeuron in neurons)
            {
                foreach (Neuron inputneuron in l.neurons)
                {
                    myNeuron.addInput(inputneuron);
                }
            }
            
        }
        public void randomizeAll()
        {
            foreach (Neuron n in neurons)
            {
                n.randomizeWeights();
            }
        }
        public double[] getOutputs()
        {
            int i = 0;
            double[] erg = new double[neurons.Length];
            foreach (Neuron n in neurons)
            {
                erg[i] = n.output;
                i++;
            }
            return erg;
        }
        public virtual void adjustWeights()
        {
            foreach (Neuron n in neurons)
            {
                n.adjustWeights();
            }
        }

       

    }
}
