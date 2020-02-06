using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public class RandBot : Bot
    {
        public RandBot(Game i_game, Boolean Color)
        {
            myGame = i_game;
            this.Color = Color;
        }
        override public void call()
        {
            Console.WriteLine("Randbot Called");
            List<int[]> Possiblilities =PossibleMoves();
            int[] move= Possiblilities[new Random().Next(0, Possiblilities.Count - 1)];
            myGame.MakeMove(move[0], move[1],this.Color);
        }

    }
}
