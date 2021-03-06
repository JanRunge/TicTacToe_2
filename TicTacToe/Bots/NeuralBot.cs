﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetwork;

namespace TicTacToe
{
    class NeuralBot:Bot
    {
        public Network Brain = new Network(9, new int[4] { 18,26,26,18 }, 9
                                          , new Function[] { new tanh(), new Sigmoid(), new Sigmoid(), new Sigmoid(), new Sigmoid() });

        //public Network Brain = new Network(18, new int[2] { 10, 10}, 9, new Function[] { new tanh(), new tanh(), new tanh() });
        public void createOrLoadTesData(string path = @"trainingTICTACTOE.json")
        {
            path= @"trainingTICTACTOE_new2.json";

            if (!File.Exists(path))
            {
                Console.WriteLine("Creating Testdata for Neuralbot");
                
                Console.WriteLine("converting to Trainingsset Object ...");
                this.Brain.trainingsset = CreateTrainingsSet();
                Console.Write("Created. ");
                Console.WriteLine(" Writing to File " + path);
               
                this.Brain.trainingsset.toFile(path);
            }
            else
            {
                Console.WriteLine("Reading Testdata from file "+path);
                this.Brain.trainingsset = TrainingSet.getFromFile(path);
            }
            Console.WriteLine("finished");
        }
        public void Train()
        {
            if (this.Brain.trainingsset == null)
            {
                createOrLoadTesData();
            }
            Brain.trainWithLogging(600,0.1,1000);
        }
        override public void call()
        {
            Console.WriteLine("NeuroBot Called");
            double[] inputs= UnpreparedTrainingsset.boardToNeuronInput(this.myGame.Gameboard, this.myGame.turn);
            int[] move = UnpreparedTrainingsset.NeuronOutputToMove(this.Brain.calculateForInput(inputs));

            //Console.WriteLine("doing"+"|"+move[1]+"|"+move[2]);
            myGame.MakeMove((int)move[0], (int)move[1], this.Color);
        }

        public static bool? numToBool(int a)
        {
            bool? b = true;
            switch (a)
            {
                case 0:
                    b = true;
                    break;
                case 1:
                    b = false;
                    break;
                case 2:
                    b = null;
                    break;
            }
            return b;
        }
        public TrainingSet CreateTrainingsSet()
        {
            List<UnpreparedTrainingsset> result = new List<UnpreparedTrainingsset>();
            AlgoBot algobot = new AlgoBot();
            Game i_game = new Game();
            i_game.turn = false;
            List<String> lstr = new List<String>();
            result.AddRange(getUnpreparedTrainingssets(i_game, lstr, algobot));
            //for the other color
            Console.WriteLine("Created Testdata for first Color");
            i_game = new Game();
            i_game.turn = true;
            result.AddRange(getUnpreparedTrainingssets(i_game, lstr, algobot));
            Console.WriteLine("Raw testdata created. " + result.Count + " sets");

            return UnpreparedTrainingsset.toTrainingsset(result);
           
        }
        private List<UnpreparedTrainingsset> getUnpreparedTrainingssets(Game i_game, List<String> lstr, AlgoBot algobot)
        {
            List<UnpreparedTrainingsset> result = new List<UnpreparedTrainingsset>();
            Game n_game;
            if (!lstr.Contains(Game.BoardToString(i_game.Gameboard, i_game.turn)))
            {
                if (!i_game.over)//only save anything if there is a playble move afterwards
                {
                    int[] bestmove = algobot.findBestMove(i_game, i_game.turn);
                    UnpreparedTrainingsset ut = new UnpreparedTrainingsset();
                    ut.inputGame = i_game.Gameboard;
                    ut.turn = i_game.turn;
                    ut.outputMove = bestmove;
                    result.Add(ut);
                    List<int[]> possibleMove = this.PossibleMoves(i_game);
                    for (int i = 0; i < possibleMove.Count; i++)
                    {
                        n_game = i_game.copy();
                        bool b = n_game.MakeMove(possibleMove[i][0], possibleMove[i][1], n_game.turn);
                        if (!b)
                        {
                            throw new Exception("nana");
                        }
                        List<UnpreparedTrainingsset> lst = getUnpreparedTrainingssets(n_game, lstr, algobot);
                        foreach (UnpreparedTrainingsset l in lst)
                        {
                            result.Add(l);
                            lstr.Add(Game.BoardToString(l.inputGame, i_game.turn));
                        }
                    }
                }
            }
            return result;
        }
        public int[] getBestMoveAccordingToTestSet(bool?[,] board, bool turn)
        {
            TrainingSet t = this.Brain.trainingsset;
            double[] input = UnpreparedTrainingsset.boardToNeuronInput(board, turn);

            int i = 0;
            while (i < t.inputs.Length)
            {
                if (input.SequenceEqual(t.inputs[i]))
                {
                    return UnpreparedTrainingsset.NeuronOutputToMove(t.results[i]);
                }
                i++;
            }
            return new int[] { -1, -1 };
        }



    }
}
