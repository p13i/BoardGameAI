using NUnit.Framework;
using TicTacToe;
using System.Linq;

namespace TicTacToeTests
{
    public class AIPlayerTests : TicTacToePackageTests
    {
        [Test]
        public void Test_GetChildStates_EmptyBoard()
        {
            (Move, TicTacToe.TicTacToe)[] expected = {
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

            (Move, TicTacToe.TicTacToe)[] children = AIPlayer.GetChildGameStates(Game).ToArray();

            Assert.IsNotNull(children);
            CollectionAssert.AreEqual(expected, children);
        }

        [Test]
        public void Test_GetChildStates_FourMoves()
        {
            Assert.IsTrue(Game.TryMove(new Move(X, new Coordinate(2, 2))));
            Assert.IsTrue(Game.TryMove(new Move(O, new Coordinate(1, 1))));
            Assert.IsTrue(Game.TryMove(new Move(X, new Coordinate(3, 1))));
            Assert.IsTrue(Game.TryMove(new Move(O, new Coordinate(1, 2))));

            (Move, TicTacToe.TicTacToe)[] expected = {
                GetChildState(X, new Coordinate(1, 3)),
                GetChildState(X, new Coordinate(2, 1)),
                GetChildState(X, new Coordinate(2, 3)),
                GetChildState(X, new Coordinate(3, 2)),
                GetChildState(X, new Coordinate(3, 3)),
            };

            (Move, TicTacToe.TicTacToe)[] children = AIPlayer.GetChildGameStates(Game).ToArray();

            Assert.IsNotNull(children);
            CollectionAssert.AreEqual(expected, children);
        }

        [Test]
        public void Test_GetChildStates_EightMoves()
        {
            Assert.IsTrue(Game.TryMove(new Move(X, new Coordinate(2, 2))));
            Assert.IsTrue(Game.TryMove(new Move(O, new Coordinate(1, 1))));
            Assert.IsTrue(Game.TryMove(new Move(X, new Coordinate(3, 1))));
            Assert.IsTrue(Game.TryMove(new Move(O, new Coordinate(1, 2))));
            Assert.IsTrue(Game.TryMove(new Move(X, new Coordinate(2, 1))));
            Assert.IsTrue(Game.TryMove(new Move(O, new Coordinate(2, 3))));
            Assert.IsTrue(Game.TryMove(new Move(X, new Coordinate(3, 3))));
            Assert.IsTrue(Game.TryMove(new Move(O, new Coordinate(3, 2))));

            (Move, TicTacToe.TicTacToe)[] expected = {
                GetChildState(X, new Coordinate(1, 3)),
            };

            (Move, TicTacToe.TicTacToe)[] children = AIPlayer.GetChildGameStates(Game).ToArray();

            Assert.IsNotNull(children);
            CollectionAssert.AreEqual(expected, children);
        }

        [Test]
        public void Test_GetChildStates_NineMoves()
        {
            Assert.IsTrue(Game.TryMove(new Move(X, new Coordinate(2, 2))));
            Assert.IsTrue(Game.TryMove(new Move(O, new Coordinate(1, 1))));
            Assert.IsTrue(Game.TryMove(new Move(X, new Coordinate(3, 1))));
            Assert.IsTrue(Game.TryMove(new Move(O, new Coordinate(1, 2))));
            Assert.IsTrue(Game.TryMove(new Move(X, new Coordinate(2, 1))));
            Assert.IsTrue(Game.TryMove(new Move(O, new Coordinate(2, 3))));
            Assert.IsTrue(Game.TryMove(new Move(X, new Coordinate(3, 3))));
            Assert.IsTrue(Game.TryMove(new Move(O, new Coordinate(3, 2))));
            Assert.IsTrue(Game.TryMove(new Move(X, new Coordinate(1, 3))));

            (Move, TicTacToe.TicTacToe)[] expected = { };

            (Move, TicTacToe.TicTacToe)[] children = AIPlayer.GetChildGameStates(Game).ToArray();

            Assert.IsNotNull(children);
            CollectionAssert.AreEqual(expected, children);

            Assert.IsTrue(Game.IsGameOver(out Player winningPlayer));

            Assert.AreEqual(X, winningPlayer);

            /**
             * |X|O|X|
             * |O|X|O|
             * |O|X|X|
             */
        }

        private (Move, TicTacToe.TicTacToe) GetChildState(Player player, Coordinate coordinate)
        {
            TicTacToe.TicTacToe game = Game.Clone();
            Move move = new Move(player, coordinate);
            Assert.IsTrue(game.TryMove(move));
            return (move, game);
        }

        [Test]
        public void Test_Evaluation_PlayerWins()
        {
            Assert.IsTrue(Game.TryMove(new Move(X, new Coordinate(1, 3))));
            Assert.IsTrue(Game.TryMove(new Move(O, new Coordinate(1, 2))));
            Assert.IsTrue(Game.TryMove(new Move(X, new Coordinate(2, 3))));
            Assert.IsTrue(Game.TryMove(new Move(O, new Coordinate(2, 2))));
            Assert.IsTrue(Game.TryMove(new Move(X, new Coordinate(3, 3))));

            Assert.IsTrue(Game.IsGameOver(out Player winningPlayer));
            Assert.AreEqual(X, winningPlayer);

            Assert.AreEqual(100, AIPlayer.Evaluation(Game, X));
            // expected = -100 [loss of game] + 10 [bonus for center] * 4 [number of windows that contain center]
            Assert.AreEqual(-100 + 10 * 4, AIPlayer.Evaluation(Game, O));
        }

        [Test]
        public void Test_GetNextMove()
        {
            Assert.IsTrue(X is AIPlayer);

            AIPlayer x = (AIPlayer)X;

            Move expected = new Move(X, new Coordinate(2, 2));
            Move actual = x.GetNextMove(Game);

            Assert.AreEqual(expected, actual, "The AI should choose to play in the center first");
        }
    }
}
