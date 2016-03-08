using System;

namespace Shortcoder.Tests.DummyShortcodes
{
    public class DummyAdvancedShortcode : Shortcode
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public override string Generate(IShortcodeContext context)
        {
            if (!string.IsNullOrEmpty(Content)) { 
                return string.Format("Hello {0}, {1} ({2})", Name, Age, Content);
            }

            return string.Format("Hello {0}, {1}", Name, Age);
        }
    }
}