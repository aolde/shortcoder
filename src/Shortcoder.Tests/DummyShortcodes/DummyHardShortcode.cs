namespace Shortcoder.Tests.DummyShortcodes
{
    public class DummyHardShortcode : Shortcode
    {
        public string Name { get; set; }
        public int? Age { get; set; }

        public override string Generate(IShortcodeContext context)
        {
            return string.Format("Dummy Hard, and you are {0}, age {1}.", Name, Age);
        }
    }
}