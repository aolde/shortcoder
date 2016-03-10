using Shortcoder.Tokenizer.Tokens;
using System.Collections.Generic;

namespace Shortcoder.Tokenizer
{
    public class Lexer : ILexer
    {
        public IEnumerable<Token> Tokenize(string source)
        {
            return null;
        }
    }

    internal interface ILexer
    {
        IEnumerable<Token> Tokenize(string source);
    }
}