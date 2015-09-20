namespace Shortcoder
{
    public static class ShortcodeConfiguration
    {
        static ShortcodeConfiguration()
        {
            Provider = new ShortcodeProvider();
            ParserFactory = new ShortcodeParserFactory();
        }

        public static IShortcodeProvider Provider { get; set; }
        public static IShortcodeParserFactory ParserFactory { get; set; }
    }
}