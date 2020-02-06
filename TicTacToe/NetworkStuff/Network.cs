using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NeuralNetwork
{
    class Network
    {
        protected InputLayer InputLayer;
        protected HiddenLayer[] hiddenLayer;
        protected OutputLayer outputLayer;
        public string type;

        public TrainingSet trainingsset;
        private void validateTraingsset()
        {
            if (this.trainingsset == null)
            {
                throw new Exception("No Traingsset Provided");
            }
            if (this.trainingsset.inputs.Length != this.trainingsset.results.Length)
            {
                throw new Exception("Length of inputs and results in trainingsset dont match");
            }
        }
        public Network(int InputLayerSize, int[] HiddenLayerSizes, int OutputLayerSize, Function[] activationFuncs )
        {
            Random r = new Random();
            //will create a network with InputLayerSize inputneurons, HiddenLayerSizes.count Hiddenlayers (Each with the size of the array at that position), and OutputLayerSize outputneurons
            /*****************************
             InputNeurons
             ****************************/
            InputLayer = new InputLayer(InputLayerSize);
            /*****************************
             HiddenNeurons
             ****************************/

            hiddenLayer = new HiddenLayer[HiddenLayerSizes.Length];

            for (int i = 0; i < hiddenLayer.Length; i++) //for each layer
            {

                hiddenLayer[i] = new HiddenLayer(HiddenLayerSizes[i]);//the size of the Layer is given in the input param
                if (i == 0)
                {
                    hiddenLayer[i].connectToInputLayer(InputLayer);
                }
                else {
                    hiddenLayer[i].connectToInputLayer(hiddenLayer[i-1]);
                }
                hiddenLayer[i].ActivationFunction = activationFuncs[i];
             }

            /*****************************
             OutputNeurons
             ****************************/
            outputLayer = new OutputLayer(OutputLayerSize);
            outputLayer.connectToInputLayer(hiddenLayer[HiddenLayerSizes.Length - 1]);
            outputLayer.ActivationFunction = activationFuncs[activationFuncs.Length-1];
            this.RandomizeAllWeights();
        }
       
        
        public void RandomizeAllWeights()//randomize weights
        {
            InputLayer.randomizeAll();
            foreach(Layer h in hiddenLayer)
            {
                h.randomizeAll();
            }
            outputLayer.randomizeAll();
        }
        /*
        public void Save(String path)
        {
            XElement xDocumentHead = new XElement("Network");

            XElement xInputLayer = new XElement("Input");
            XElement xHiddenLayers = new XElement("Hidden");
            XElement xOuputLayer = new XElement("Output");
            XElement xAxons = new XElement("Axons");
            xDocumentHead.Add(xInputLayer);
            xDocumentHead.Add(xHiddenLayers);
            xDocumentHead.Add(xOuputLayer);
            xDocumentHead.Add(xAxons);

            foreach (Neuron n in InputNeurons)
            {
                XAttribute xType = new XAttribute("Type", "input");
                XAttribute xName = new XAttribute("Name", n.name);
                XElement xn = new XElement("Neuron", xType, xName);
                xInputLayer.Add(xn);
            }
            foreach (Neuron[] Layer in hiddenNeurons)
            {
                XElement xHiddenLayer = new XElement("Layer");
                xHiddenLayers.Add(xHiddenLayer);
                foreach (Neuron n in Layer)
                {
                    XAttribute xType = new XAttribute("Type", "hidden");
                    XAttribute xName = new XAttribute("Name", n.name);
                    XElement xn = new XElement("Neuron", xType, xName);


                    xHiddenLayer.Add(xn);
                }
            }

            foreach (Neuron n in outputNeurons)
            {
                XAttribute xType = new XAttribute("Type", "output");
                XAttribute xName = new XAttribute("Name", n.name);
                XElement xn = new XElement("Neuron", xType, xName);
                xOuputLayer.Add(xn);
            }
            xDocumentHead.Save("Root.xml");



        }*/

        public double[] calculateForInput(double[] inputs)
        {
            
            this.setInputs(inputs);
            this.fire();
            return getResult();
            
        }
        protected double[] getResult()
        {
            return outputLayer.getOutputs();
        }
        protected void fire()
        {
            InputLayer.fire();
            
            foreach (HiddenLayer Layer in this.hiddenLayer)
            {
                Layer.fire();
                
            }
            outputLayer.fire();
        }
        protected void setInputs(double[] inputs)
        {
            int i = 0;
            foreach (InputNeuron n in this.InputLayer.neurons)
            {
                n.input = inputs[i];
                i++;
            }
        }
        protected double calculateErrors(double[] desiredResult)
        {
            outputLayer.setDesiredResults(desiredResult);
            double errorsum= outputLayer.calculateErrors();
            for (int l = 0; l < hiddenLayer.Count(); l++)
            {
                hiddenLayer[l].calculateErrors();
            }
            return errorsum;
        }
        protected void asjustWeights()
        {
            foreach (HiddenLayer Layer in hiddenLayer)
            {
                Layer.adjustWeights();
            }
            outputLayer.adjustWeights();
        }
        public void train(double faultTolerance, int maxEpochs)
        {
            validateTraingsset();
            int epoch = 0;
            bool abortflag = false;
            double avgError = 1;
            double errorSum;
            while (epoch <= maxEpochs && !abortflag)
            {
                double[] absoluteErrors = new double[outputLayer.neurons.Count()];
                for (int i = 0; i < outputLayer.neurons.Count(); i++)
                {
                    absoluteErrors[i] = 0f;

                }
                    errorSum = 0;
                epoch++;
                for (int i = 0; i < this.trainingsset.inputs.Length; i++)  //Loop over every input dataset
                {
                    setInputs(this.trainingsset.inputs[i]);
                    this.fire();
                    errorSum += this.calculateErrors(this.trainingsset.results[i]);
                    this.asjustWeights();
                    double[] betw = outputLayer.getAbsoluteErrors();
                    absoluteErrors = add(absoluteErrors,betw );
                }
                //print(absoluteErrors);
                print(div(absoluteErrors, this.trainingsset.inputs.Length));
                if (epoch % 250 == 0)
                {
                    Console.WriteLine("Epoch " + epoch + "current error=" + avgError);
                }
                if ( avgError < errorSum / trainingsset.inputs.Length)
                {
                    Console.WriteLine("Epoch " + epoch + "current error=" + avgError + " the error is ingreasing.");
                }

                avgError = errorSum / trainingsset.inputs.Length;
            }/*
            if (abortflag)
            {
                Console.WriteLine("Training was aborted. Current Error: " + highestError);
            }else if (epoch >= maxEpochs)
            {
                Console.WriteLine("Training finished all Epochs ("+epoch+"). Remaining Error:" + highestError);
            }else
            {
                
            }*/
            Console.WriteLine("Epoch " + epoch + " Training ended. Error= " + avgError);
        }

        public void test()
        {
            for (int i = 0; i < this.trainingsset.inputs.Length; i++)  //Loop over every input dataset
            {
                double[] erg = this.calculateForInput(this.trainingsset.inputs[i]);
                Console.WriteLine("------ ");
                Console.WriteLine(trainingsset.results.ToString() + " ");
                Console.WriteLine(erg.ToString() + " ");
                Console.WriteLine("------ ");
            }
        }
        void print(double[] thing)
        {
            for(int i=0; i< thing.Length; i++)
            {
                Console.Write(thing[i]+" | ");

            }
            Console.WriteLine("");
        }
        double[] add(double[] a , double[] b)
        {
            for (int i = 0; i < a.Length; i++)
            {
                b[i] = (double)(b[i]+a[i]);

            }
            return b;
        }
        double[] div(double[] a, int b)
        {
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = a[i]/b;

            }
            return a;
        }

    }
}
