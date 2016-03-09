namespace Shortcoder.Tests.DummyShortcodes
{
    public class DummyListItemShortcode : Shortcode
    {
        public override string Generate(IShortcodeContext context)
        {
            return "Item" + Content;
        }
    }
}