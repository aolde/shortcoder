using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Shortcoder.Tokenizer.Tokens
{
    public abstract class Token
    {
        public TokenPosition Position { get; set; }

        public string Value { get; set; }
    }
}
