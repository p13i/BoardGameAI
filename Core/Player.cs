using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGameAI.Core
{
    public abstract class Player<TToken>
    {
        public Player(string name, TToken token)
        {
            Name = name;
            Token = token;
        }

        public string Name { get; }
        public TToken Token { get; }

        public override int GetHashCode()
        {
            return Name.GetHashCode() ^ Token.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Player<TToken> p = (Player<TToken>)obj;
                return Name.Equals(p.Name) && Token.Equals(p.Token);
            }
        }

        public override string ToString()
        {
            return $"{Name} ({Token.ToString()})";
        }

        public abstract Move<TToken> GetNextMove(IBoardGame<TToken> game);

        public abstract Player<TToken> Clone();
    }
}
