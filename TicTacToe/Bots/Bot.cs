using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class Bot
    {
        public Game myGame;
        public bool Color;
        public virtual void call()
        {

        }
        public List<int[]> PossibleMoves()
        {
            return PossibleMoves(this.myGame);
        }
        public List<int[]> PossibleMoves(Game i_game)
        {
            List<int[]> Possiblilities = new List<int[]>();
            for (int i = 0; i < 3; i++)
            {
                for (int k = 0; k < 3; k++)
                {
                    if (i_game.Gameboard[i, k] == null)
                    {
                        Possiblilities.Add(new int[] { i, k });
                    }
                }
            }
            return Possiblilities;
        }
    }
}
