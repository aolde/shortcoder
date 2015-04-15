namespace Shortcoder.Tests.DummyShortcodes
{
    public class DummySmallShortcode : Shortcode
    {
        public override string Generate(IShortcodeContext context)
        {
            return "From Dummy Small";
        }
    }
}
