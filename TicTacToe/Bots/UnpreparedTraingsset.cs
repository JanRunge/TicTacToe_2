using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetwork;

namespace TicTacToe
{
    public class UnpreparedTrainingsset
    {
        public bool?[,] inputGame;
        public bool turn;
        public int[] outputMove;

       

        public static TrainingSet toTrainingsset(List<UnpreparedTrainingsset> sets)
        {
            double[][] inputs = new double[sets.Count][];
            double[][] outputs = new double[sets.Count][];
            int i = 0;
            foreach (UnpreparedTrainingsset set in sets)
            {
                inputs[i] = boardToNeuronInput(set.inputGame, set.turn);
                outputs[i] = moveToNeuronOutput(set.outputMove);
                i++;
            }
            TrainingSet t = new TrainingSet();
            t.inputs = inputs;
            t.results = outputs;
            return t;
        }
        public static double[] moveToNeuronOutput(int[] move)
        {
            double[] output = new double[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int index = move[0] *3 + move[1];
            output[index] = 1;
            return output;
        }
        public static int[] NeuronOutputToMove(double[] output)
        {
            int indexOfOne = -1;
            double maxval = -9999;
            for(int i=0; i<9; i++)
            {
                if (output[i] > maxval)
                {
                    indexOfOne = i;
                    maxval = output[i];
                }
            }
            if (indexOfOne == -1)
            {
                throw new Exception("no valid neuron found");
            }
            int[] move = new int[2];
            move[0] = (int) Math.Floor((double)indexOfOne / 3);
            move[1] = indexOfOne % 3;
            return move;

        }
        public static double[] boardToNeuronInput(bool?[,] board, bool turn)
        {                                  //00,00,01,01,02,02,10,10,11,11,
            

            double[] input = new double[18] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for(int i=0; i<3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    double valOne;
                    double valTwo;
                    if (board[i, j] == null)
                    {
                        valOne = 0;
                        valTwo = 0;
                    }else if (board[i, j] == turn)
                    {
                        valOne = 1;
                        valTwo = 0;
                    }else
                    {
                        valOne = 0;
                        valTwo = 1;
                    }
                    input[i+9 + j]= valOne;
                    input[i + j]= valTwo;
                }
            }

            return input;
        }

    }
}
