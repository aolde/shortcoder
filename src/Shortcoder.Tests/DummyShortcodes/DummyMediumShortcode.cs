namespace Shortcoder.Tests.DummyShortcodes
{
    public class DummyMediumShortcode : Shortcode
    {
        public override string Generate(IShortcodeContext context)
        {
            var name = Attributes["name"];

            if (IsSet("age"))
            {
                var age = Attributes["age"];
                return string.Format("Dummy Medium, and you are {0}, age {1}.", name, age);                                
            }
            
            return string.Format("Dummy Medium, and you are {0}.", name);
        }
    }
}