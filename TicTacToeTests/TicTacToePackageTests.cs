using System;
using System.Collections.Generic;
using System.Text;
using BoardGameAI.Core;
using TicTacToe;
using NUnit.Framework;
using TicTacToe.Game;

namespace TicTacToeTests
{
    public class TicTacToePackageTests
    {
        protected Player<TicTacToeToken> X;
        protected Player<TicTacToeToken> O;
        protected TicTacToe.TicTacToeGame Game;

        [SetUp]
        public void Setup()
        {
            X = new MinimaxTicTacToePlayer("player-one (AI)", TicTacToeToken.X);
            O = new HumanTicTacToePlayer("player-two (Human)", TicTacToeToken.O);
            Game = new TicTacToe.TicTacToeGame(X, O);
        }
    }
}
