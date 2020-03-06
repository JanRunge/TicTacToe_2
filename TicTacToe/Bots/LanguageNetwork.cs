using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetwork;

namespace TicTacToe
{
    class LanguageNetwork
    {
        public Network myNetwork = new Network(35, new int[2] { 25, 15 }, 1, new Function[3] { new tanh(), new tanh(), new Sigmoid() });

        public void getTrainingsset()
        {
            String trainingDataFile = @"trainng_italienisch_1.csv";
            TrainingSet trainset= new TrainingSet();
            int inputLength = 35;
            int outputLength = 1;
            double[] input = new double[inputLength];
            double[] outputErwartet = new double[outputLength];

            if (File.Exists(trainingDataFile))
            {
                string[] lines = File.ReadAllLines(trainingDataFile);
                trainset.inputs = new double[lines.Length][];
                trainset.results = new double[lines.Length][];

                string[][] parts = new string[lines.Length][];
                for (int i = 0; i < lines.Length; i++)
                {

                    parts[i] = lines[i].Split(';');

                    input = getUTFD100ByteArray(parts[i][0], inputLength);
                    //output = getNetOutput(net, parts[i][0]);



                    outputErwartet = new double[outputLength];
                    Array.Clear(outputErwartet, 0, outputLength);
                    for (int k = 1; k <= outputLength; k++)
                    {
                        outputErwartet[k - 1] = double.Parse(parts[i][k]);
                        //Console.WriteLine("Erwarteter Wert für: output[" + (k - 1) + "]: " + double.Parse(parts[i][k]));
                    }
                    trainset.inputs[i] = input;
                    trainset.results[i] = outputErwartet;
                }

            }
            else
            {
                throw new Exception("");
            }
            myNetwork.trainingsset = trainset;
        }
        static double[] getUTFD100ByteArray(String zeichenkette, int eingabeLaenge)
        {
            String byteConverted;
            double[] input = new double[eingabeLaenge];
            Array.Clear(input, 0, eingabeLaenge);

            for (int j = 0; j < zeichenkette.Length; j++)//Jedes eingelesene Zeichen durchlaufen, Spracheingabe steht immer an Position 0
            {
                byteConverted = Convert.ToInt32(zeichenkette[j]).ToString("x");
                int zahlenwert = Int32.Parse(byteConverted, System.Globalization.NumberStyles.HexNumber);
                input[j] = zahlenwert / 100.0d;
                //Console.WriteLine("zahlenwert= " + zahlenwert + ", input[" + zeichen + "] = " + input[zeichen]);
                if (j == eingabeLaenge) //spätestens Aussteigen, wenn kompletter Input-Vektor belegt
                {
                    break;
                }
            }
            return input;
        }

        public void train()
        {
            myNetwork.trainWithLogging(100000, 1000);
        }
    }
}
