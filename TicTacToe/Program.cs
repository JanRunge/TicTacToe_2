using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TicTacToe
{
    class Program
    {
        static void Main(string[] args)
        {

            NeuralBot n = new NeuralBot();
            Console.WriteLine("starting calc");
            List<NeuralBot.UnpreparedTrainingsset> t = n.getAllPossibleStatesWithRedundancy(null,null,null);
            Console.WriteLine(t.Count);
            Console.WriteLine("Ready");
            


            Console.ReadLine();
            
        }
        static void runGame()
        {
            Game g = new Game();
            AlgoBot a = new AlgoBot(true, g);
            while (g.winner == null)
            {
                g.print();
                if (g.turn == a.Color)
                {
                    Thread.Sleep(1000);
                    a.call();
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
    }
}
