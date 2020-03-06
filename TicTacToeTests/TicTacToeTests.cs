using BoardGameAI.Core;
using NUnit.Framework;
using TicTacToe;

namespace TicTacToeTests
{
    public class TicTacToeTests : TicTacToePackageTests
    {
        [Test]
        public void Test_IsGameOver_PlayerOneWins()
        {
            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(X, new Coordinate(2, 2))));
            Assert.IsFalse(Game.IsGameOver(out _));

            /*
             * | | | |
             * | |X| |
             * | | | |
             */

            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(O, new Coordinate(1, 1))));
            Assert.IsFalse(Game.IsGameOver(out _));

            /*
             * | | | |
             * | |X| |
             * |O| | |
             */

            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(X, new Coordinate(1, 2))));
            Assert.IsFalse(Game.IsGameOver(out _));

            /*
             * | | | |
             * |X|X| |
             * |O| | |
             */

            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(O, new Coordinate(1, 3))));
            Assert.IsFalse(Game.IsGameOver(out _));

            /*
             * |O| | |
             * |X|X| |
             * |O| | |
             */

            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(X, new Coordinate(3, 2))));
            Assert.IsTrue(Game.IsGameOver(out Player<TicTacToeToken> winningPlayer));
            Assert.AreEqual(X, winningPlayer);

            /*
             * |O| | |
             * |X|X|X|
             * |O| | |
             */

        }

        [Test]
        public void Test_IsGameOver_Draw()
        {
            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(X, new Coordinate(2, 2))));
            Assert.IsFalse(Game.IsGameOver(out _));

            /*
             * | | | |
             * | |X| |
             * | | | |
             */

            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(O, new Coordinate(1, 1))));
            Assert.IsFalse(Game.IsGameOver(out _));

            /*
             * | | | |
             * | |X| |
             * |O| | |
             */

            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(X, new Coordinate(1, 2))));
            Assert.IsFalse(Game.IsGameOver(out _));

            /*
             * | | | |
             * |X|X| |
             * |O| | |
             */

            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(O, new Coordinate(1, 3))));
            Assert.IsFalse(Game.IsGameOver(out _));

            /*
             * |O| | |
             * |X|X| |
             * |O| | |
             */


            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(X, new Coordinate(2, 3))));
            Assert.IsFalse(Game.IsGameOver(out _));

            /*
             * |O|X| |
             * |X|X| |
             * |O| | |
             */

            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(O, new Coordinate(2, 1))));
            Assert.IsFalse(Game.IsGameOver(out _));

            /*
             * |O|X| |
             * |X|X| |
             * |O|O| |
             */

            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(X, new Coordinate(3, 3))));
            Assert.IsFalse(Game.IsGameOver(out _));

            /*
             * |O|X|X|
             * |X|X| |
             * |O|O| |
             */

            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(O, new Coordinate(3, 2))));
            Assert.IsFalse(Game.IsGameOver(out _));

            /*
             * |O|X|X|
             * |X|X|O|
             * |O|O| |
             */

            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(X, new Coordinate(3, 1))));
            Assert.IsTrue(Game.IsGameOver(out Player<TicTacToeToken> winningPlayer));
            Assert.IsNull(winningPlayer);

            /*
             * |O|X|X|
             * |X|X|O|
             * |O|O|X|
             */
        }

    }
}