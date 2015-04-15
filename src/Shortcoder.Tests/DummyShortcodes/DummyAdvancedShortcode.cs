namespace Shortcoder.Tests.DummyShortcodes
{
    public class DummyAdvancedShortcode : Shortcode
    {
        public string Name { get; set; }
        public int Age { get; set; }

        public override string Generate(IShortcodeContext context)
        {
            //var parsedContent = context.Parser.Parse(Content);
            return string.Format("Hello {0}, {1} ({2})", Name, Age, Content);
        }
    }
}