using System;
using System.Collections.Generic;
using System.Text;
using TicTacToe;
using NUnit.Framework;

namespace TicTacToeTests
{
    public class TicTacToePackageTests
    {
        protected Player X;
        protected Player O;
        protected TicTacToe.TicTacToe Game;

        [SetUp]
        public void Setup()
        {
            X = new AIPlayer("player-one (AI)", Token.X);
            O = new HumanPlayer("player-two (Human)", Token.O);
            Game = new TicTacToe.TicTacToe(X, O);
        }
    }
}
