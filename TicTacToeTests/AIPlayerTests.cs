using NUnit.Framework;
using TicTacToe;
using System.Linq;
using BoardGameAI.Core;
using TicTacToe.Game;

namespace TicTacToeTests
{
    public class AIPlayerTests : TicTacToePackageTests
    {
        [Test]
        public void Test_GetChildStates_EmptyBoard()
        {
            (Move<TicTacToeToken>, TicTacToeGame)[] expected = {
                GetChildState(X, new Coordinate(1, 1)),
                GetChildState(X, new Coordinate(1, 2)),
                GetChildState(X, new Coordinate(1, 3)),
                GetChildState(X, new Coordinate(2, 1)),
                GetChildState(X, new Coordinate(2, 2)),
                GetChildState(X, new Coordinate(2, 3)),
                GetChildState(X, new Coordinate(3, 1)),
                GetChildState(X, new Coordinate(3, 2)),
                GetChildState(X, new Coordinate(3, 3)),
            };

            (Move<TicTacToeToken>, IMinimaxGame<TicTacToeToken>)[] children = Game.GetChildGameStates().ToArray();

            Assert.IsNotNull(children);
            CollectionAssert.AreEqual(expected, children);
        }

        [Test]
        public void Test_GetChildStates_FourMoves()
        {
            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(X, new Coordinate(2, 2))));
            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(O, new Coordinate(1, 1))));
            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(X, new Coordinate(3, 1))));
            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(O, new Coordinate(1, 2))));

            (Move<TicTacToeToken>, TicTacToeGame)[] expected = {
                GetChildState(X, new Coordinate(1, 3)),
                GetChildState(X, new Coordinate(2, 1)),
                GetChildState(X, new Coordinate(2, 3)),
                GetChildState(X, new Coordinate(3, 2)),
                GetChildState(X, new Coordinate(3, 3)),
            };

            (Move<TicTacToeToken>, IMinimaxGame<TicTacToeToken>)[] children = Game.GetChildGameStates().ToArray();

            Assert.IsNotNull(children);
            CollectionAssert.AreEqual(expected, children);
        }

        [Test]
        public void Test_GetChildStates_EightMoves()
        {
            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(X, new Coordinate(2, 2))));
            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(O, new Coordinate(1, 1))));
            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(X, new Coordinate(3, 1))));
            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(O, new Coordinate(1, 2))));
            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(X, new Coordinate(2, 1))));
            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(O, new Coordinate(2, 3))));
            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(X, new Coordinate(3, 3))));
            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(O, new Coordinate(3, 2))));

            (Move<TicTacToeToken>, TicTacToeGame)[] expected = {
                GetChildState(X, new Coordinate(1, 3)),
            };

            (Move<TicTacToeToken>, IMinimaxGame<TicTacToeToken>)[] children = Game.GetChildGameStates().ToArray();

            Assert.IsNotNull(children);
            CollectionAssert.AreEqual(expected, children);
        }

        [Test]
        public void Test_GetChildStates_NineMoves()
        {
            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(X, new Coordinate(2, 2))));
            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(O, new Coordinate(1, 1))));
            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(X, new Coordinate(3, 1))));
            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(O, new Coordinate(1, 2))));
            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(X, new Coordinate(2, 1))));
            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(O, new Coordinate(2, 3))));
            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(X, new Coordinate(3, 3))));
            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(O, new Coordinate(3, 2))));
            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(X, new Coordinate(1, 3))));

            (Move<TicTacToeToken>, TicTacToeGame)[] expected = { };

            (Move<TicTacToeToken>, IMinimaxGame<TicTacToeToken>)[] children = Game.GetChildGameStates().ToArray();

            Assert.IsNotNull(children);
            CollectionAssert.AreEqual(expected, children);

            Assert.IsTrue(Game.IsGameOver(out Player<TicTacToeToken> winningPlayer));

            Assert.AreEqual(X, winningPlayer);

            /**
             * |X|O|X|
             * |O|X|O|
             * |O|X|X|
             */
        }

        private (Move<TicTacToeToken>, TicTacToeGame) GetChildState(Player<TicTacToeToken> player, Coordinate coordinate)
        {
            TicTacToeGame game = (TicTacToeGame) Game.Clone();
            Move<TicTacToeToken> move = new Move<TicTacToeToken>(player, coordinate);
            Assert.IsTrue((bool)game.TryMove(move));
            return (move, game);
        }

        [Test]
        public void Test_Evaluation_PlayerWins()
        {
            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(X, new Coordinate(1, 3))));
            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(O, new Coordinate(1, 2))));
            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(X, new Coordinate(2, 3))));
            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(O, new Coordinate(2, 2))));
            Assert.IsTrue(Game.TryMove(new Move<TicTacToeToken>(X, new Coordinate(3, 3))));

            Assert.IsTrue(Game.IsGameOver(out Player<TicTacToeToken> winningPlayer));
            Assert.AreEqual(X, winningPlayer);

            Assert.AreEqual(100, Game.Evaluation(X));
            // expected = -100 [loss of game] + 10 [bonus for center] * 4 [number of windows that contain center]
            Assert.AreEqual(-80 + 10 * 4, Game.Evaluation(O));
        }

        [Test]
        public void Test_GetNextMove()
        {
            Assert.IsTrue(X is MinimaxTicTacToePlayer);

            MinimaxTicTacToePlayer x = (MinimaxTicTacToePlayer)X;

            Move<TicTacToeToken> expected = new Move<TicTacToeToken>(X, new Coordinate(2, 2));
            Move<TicTacToeToken> actual = x.GetNextMove(Game);

            Assert.AreEqual(expected, actual, "The AI should choose to play in the center first");
        }
    }
}
