﻿using System;
using System.Collections.Generic;

namespace Shortcoder.Parsing
{
    public class ShortcodeParseInfo
    {
        public ShortcodeParseInfo()
        {
            Attributes = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
        }

        public int BeginPosition { get; set; }
        public int EndPosition { get; set; }

        public string Tag { get; set; }
        public Dictionary<string, object> Attributes { get; set; }
        
        public string Content { get; set; }

        public bool IsClosed { get; set; }
        public string PreCompiledContent { get; set; }
    }
}