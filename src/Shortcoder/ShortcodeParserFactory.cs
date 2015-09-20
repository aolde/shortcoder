namespace Shortcoder
{
    public class ShortcodeParserFactory : IShortcodeParserFactory
    {
        public IShortcodeParser Create()
        {
            var provider = ShortcodeConfiguration.Provider;
            return new ShortcodeParser(provider);
        }
    }
}