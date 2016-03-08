namespace Shortcoder.Tests.DummyShortcodes
{
    public class DummyAdvancedShortcode : Shortcode
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public override string Generate(IShortcodeContext context)
        {
            //var parsedContent = context.Parser.Parse(Content);
            if (!string.IsNullOrEmpty(Content)) { 
                return $"Hello {Name}, {Age} ({Content})";
            }

            return $"Hello {Name}, {Age}";
        }
    }
}