using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    class AlgoBot : Bot
    {
        public AlgoBot(Boolean Color)
        {
            this.Color = Color;
        }
        public AlgoBot(Boolean Color, Game  g)
        {
            this.Color = Color;
            this.myGame = g;
        }
        public AlgoBot()
        {
        }
        override public void call()
        {
            Console.WriteLine("AlgoBot Called");

            //int[] move = findBestMove(this.myGame);
            double[] move = findBestMove(this.myGame);
            
            //Console.WriteLine("doing"+"|"+move[1]+"|"+move[2]);
            myGame.MakeMove((int)move[1], (int)move[2], this.Color);
        }
        int count;
        public double[] findBestMove( Game i_game)
        {
            return findBestMove(i_game, this.Color);

        }
        public double[] findBestMove(Game i_game, bool color)
        {
            count = 0;
            List<int[]> possibleMove = PossibleMoves(i_game);
            Game n_game;
            List<double[]> returnAuswahl = new List<double[]>();
            //Console.Read();
            for (var i = 0; i < possibleMove.Count; i++)
            {

                //testcommit2
                n_game = i_game.copy();
                n_game.MakeMove(possibleMove[i][0], possibleMove[i][1], i_game.turn);
                returnAuswahl.Add(new double[] { findBestMove_d(n_game), possibleMove[i][0], possibleMove[i][1] });
            }
            //print(returnAuswahl);
            //Console.WriteLine(count+" positions evaluated");
            return returnAuswahl[bestEvaluation(returnAuswahl,color)];

        }

        private void print(List<double[]> returnAuswahl)
        {
            foreach (double[] element in returnAuswahl)
            {
                Console.WriteLine(element[0] + "|" + element[1] + "|" + element[2] + "|");
            }
        }
        private double findBestMove_d(Game i_game)
        {
            count++;
            List<int[]> possibleMove = this.PossibleMoves(i_game);
            double sumPos = 0;
            if (i_game.over)
            {
                return evaluateBoard(i_game);
            }
            else
            {
                Game n_game;
                for (var i = 0; i < possibleMove.Count; i++)
                {
                    n_game = i_game.copy();
                    n_game.MakeMove(possibleMove[i][0]
                                  , possibleMove[i][1]
                                  , n_game.turn);
                    sumPos = sumPos + findBestMove_d(n_game);
                }


            }
            return sumPos / (possibleMove.Count*2);
        }


        public int evaluateBoard(Game i_game)
        {
            if (i_game.winner==true)
            {
                return 1;
            }else if(i_game.winner == false)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
        
        private int bestEvaluation(List<double[]> auswahl, Boolean Color)
        {

            double besteval = 0;
            int bestevalindex = -1;
            List<int> SelberWert = new List<int>();
            //SelberWert.Add(bestevalindex);
            if (Color == false)
            {
                besteval = 2;
                for (int i = 0; i < auswahl.Count; i++)
                {
                    if (auswahl[i][0] < besteval)
                    {
                        SelberWert.Clear();
                        besteval = auswahl[i][0];
                        bestevalindex = i;
                        SelberWert.Add(i);
                    }
                    else if (auswahl[i][0] == besteval)
                    {
                        SelberWert.Add(i);
                    }
                }
            }
            else
            {
                besteval = -2;
                for (int i = 0; i < auswahl.Count; i++)
                {
                    if (auswahl[i][0] > besteval)
                    {
                        SelberWert.Clear();
                        besteval = auswahl[i][0];
                        bestevalindex = i;
                        SelberWert.Add(bestevalindex);
                    }
                    else if (auswahl[i][0] == besteval)
                    {
                        SelberWert.Add(i);
                    }
                }
            }
            //return bestevalindex;
            return SelberWert[new Random().Next(0, SelberWert.Count - 1)];
        }
    }
}