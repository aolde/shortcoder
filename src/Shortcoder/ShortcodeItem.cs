namespace Shortcoder
{
    public class ShortcodeItem : Shortcode
    {
        public override string Generate(IShortcodeContext context)
        {
            return Content;
        }
    }
}