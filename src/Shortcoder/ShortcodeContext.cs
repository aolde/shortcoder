namespace Shortcoder
{
    public class ShortcodeContext : IShortcodeContext
    {
        public string Tag { get; set; }
        public IShortcodeParser Parser { get; set; }
    }
}