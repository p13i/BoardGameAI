using System;
using System.Collections.Generic;
using System.Text;
using BoardGameAI.Core;

namespace BoardGameAI.Core
{
    public interface IMinimaxGame<TToken> : IBoardGame<TToken>
    {
        IEnumerable<(Move<TToken>, IMinimaxGame<TToken>)> GetChildGameStates();


        /// <summary>
        /// The evaluation of the game state is measured by the number of instances that pair of the current player's tokens are found together
        /// minus the same for the opposing player
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        int Evaluation(Player<TToken> player);
    }
}
