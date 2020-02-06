using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    abstract class Neuron
    {
        public List<Axon> inputs = new List<Axon>();
        public List<Axon> outputs = new List<Axon>();
        
        public double error;
        public double bias;

        public Layer myLayer;
        public String name;
        public double output;

        public abstract void calculateError(double desired_result);
        public virtual void fire()
        {
            double sum = 0;
            for (int i = 0; i < inputs.Count; i++)
            {
                sum += (inputs[i].weight * inputs[i].input.output);
            }
            output = myLayer.ActivationFunction.calculate(sum + bias);
        }
        public void addInput(Neuron n)
        {
            new Axon(n, this);
        }

        public Neuron(Layer l)
        {
            myLayer = l;
        }

        public void randomizeWeights()
        {
            //@TODO make this dynamic
            for (int i=0; i< inputs.Count; i++)
            {
                inputs[i].weight = Global.random.NextDouble()-1;

            }
            bias = Global.random.NextDouble() - 1;//TODO: DOUBLE
        }

        public virtual void adjustWeights()
        {
            foreach (Axon a in this.inputs)
            {
                a.weight += 0.1*error*a.input.output;
            }
            bias += 0.1 *error;

        }
    }
}
