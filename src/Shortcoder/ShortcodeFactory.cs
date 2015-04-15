namespace Shortcoder
{
    public static class ShortcodeFactory
    {
        static ShortcodeFactory()
        {
            Provider = new ShortcodeProvider();
            Parser = new ShortcodeParser(Provider);
        }

        public static IShortcodeProvider Provider { get; set; }
        public static IShortcodeParser Parser { get; set; }
    }
}