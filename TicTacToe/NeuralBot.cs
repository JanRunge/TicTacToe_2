using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    class NeuralBot:Bot
    {
        public class UnpreparedTrainingsset
        {
            public bool?[,] inputGame;
            public bool turn;
            public int[] outputMove;

            public void toTraingsset()
            {

            }
        }
        public void createTestData()
        {

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
        List<bool?[,]> Rresult = new List<bool?[,]>();
        public List<bool?[,]> getAllPossibleStatesWithRedundancy(Game i_game, List<String> lstr, int q)
        {
            q++;
            

            List<bool?[,]> result = new List<bool?[,]>();
            
            Game n_game;
            if(i_game is null)
            {
                i_game = new Game();
                i_game.turn = false;
                lstr = new List<String>();
                result.AddRange(getAllPossibleStatesWithRedundancy(i_game, lstr,q));
                //for the other color
                Console.WriteLine("50%");
                i_game = new Game();
                i_game.turn = true;
                result.AddRange(getAllPossibleStatesWithRedundancy(i_game, lstr,q));

            }
            else
            {
                String bo = Game.BoardToString(i_game.Gameboard, i_game.turn);
                
                if (!lstr.Contains(bo))
                {
                    if (!i_game.over)//only save anything if there is a playble move afterwards
                    {
                        result.Add(i_game.Gameboard);
                        List<int[]> possibleMove = this.PossibleMoves(i_game);
                        List<bool?[,]> lst = new List<bool?[,]>();
                        for (int i = 0; i < possibleMove.Count; i++)
                        {
                            n_game = i_game.copy();
                            bool b = n_game.MakeMove(possibleMove[i][0], possibleMove[i][1], n_game.turn);
                            if (!b)
                            {
                                throw new Exception("nana");
                            }
                            lst = getAllPossibleStatesWithRedundancy(n_game, lstr,q);
                            
                            foreach (bool?[,] l in lst)
                            {
                                result.Add(l);
                                lstr.Add(Game.BoardToString(l, i_game.turn));
                            }

                        }
                    }
                }
            }
            

            
            return result;
        }
        public List<UnpreparedTrainingsset> getAllPossibleStatesWithRedundancy(Game i_game, List<String> lstr, AlgoBot algobot)
        {

            List<UnpreparedTrainingsset> result = new List<UnpreparedTrainingsset>();

            Game n_game;
            if (i_game is null)
            {
                algobot = new AlgoBot();
                i_game = new Game();
                i_game.turn = false;
                lstr = new List<String>();
                result.AddRange(getAllPossibleStatesWithRedundancy(i_game, lstr, algobot));
                //for the other color
                Console.WriteLine("50%");
                i_game = new Game();
                i_game.turn = true;
                result.AddRange(getAllPossibleStatesWithRedundancy(i_game, lstr, algobot));

            }
            else
            {
                if (!lstr.Contains(Game.BoardToString(i_game.Gameboard, i_game.turn)))
                {
                    if (!i_game.over)//only save anything if there is a playble move afterwards
                    {
                        double[] bestmove = algobot.findBestMove(i_game, i_game.turn);
                        UnpreparedTrainingsset ut = new UnpreparedTrainingsset();
                        ut.inputGame = i_game.Gameboard;
                        ut.turn = i_game.turn;
                        ut.outputMove = new int[2] { (int)bestmove[0], (int)bestmove[1] };
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
                            List<UnpreparedTrainingsset> lst = getAllPossibleStatesWithRedundancy(n_game, lstr, algobot);
                            foreach (UnpreparedTrainingsset l in lst)
                            {
                                result.Add(l);
                                lstr.Add(Game.BoardToString(l.inputGame, i_game.turn));
                            }

                        }
                    }
                }
            }



            return result;
        }



    }
}
