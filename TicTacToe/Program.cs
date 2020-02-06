﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NeuralNetwork;
namespace TicTacToe
{
    class Program
    {
        static void Main(string[] args)
        {
            XoRNetwork x = new XoRNetwork();
            //x.train(0.01,2000);
            NeuralBot n = new NeuralBot();
            n.Color = true;
            n.myGame = new Game();

            n.Train();
            


            Console.ReadLine();
            runGame(n);


        }
        static void runGame(Bot b)
        {
            Game g = b.myGame;
            while (g.winner == null)
            {
                g.print();
                if (g.turn == b.Color)
                {
                    Thread.Sleep(1000);
                    b.call();
                }
                else
                {
                    String userInput = Console.ReadLine();
                    int num1 = Int32.Parse(userInput.Substring(0, 1));
                    int num2 = Int32.Parse(userInput.Substring(2, 1));
                    g.MakeMove(num1, num2, false);
                }


            }
            Console.WriteLine("Winner is" + g.winner.ToString());

        }
        static void runGame()
        {
            AlgoBot a = new AlgoBot(true);
            a.myGame = new Game();
            runGame(a);

        }
    }
}
