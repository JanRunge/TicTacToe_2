using System;
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

            NeuralBot b = new NeuralBot();
            b.Train();
            b.myGame = new Game();
            runGame(b);
            Console.ReadLine();


        }
        static void runGame(Bot b)
        {
            void print(int[] list)
            {
                Console.WriteLine("Best move according to traingsset: ");
                for (int i = 0; i < list.Length; i++){
                    Console.Write(list[i] + " ");
                }
            }
            Game g = b.myGame;
            NeuralBot n = (NeuralBot)b;
            while (g.winner == null)
            {
                g.print();
                if (g.turn == b.Color)
                {
                    print(n.getBestMoveAccordingToTestSet(g.Gameboard, g.turn));
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
