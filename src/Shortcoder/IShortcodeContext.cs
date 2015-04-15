namespace Shortcoder
{
    public interface IShortcodeContext
    {
        string Tag { get; set; }
        IShortcodeParser Parser { get; set; }
    }
}