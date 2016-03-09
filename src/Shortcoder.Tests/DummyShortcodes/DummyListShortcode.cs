using System;
using System.Text;

namespace Shortcoder.Tests.DummyShortcodes
{
    public class DummyListShortcode : Shortcode
    {
        public override string Generate(IShortcodeContext context)
        {
            var childShortcodes = context.Parser.GetShortcodes(Content);

            if (childShortcodes == null) { 
                return null;
            }

            var stringBuilder = new StringBuilder();
            stringBuilder.Append("<ul>");

            foreach (var shortcode in childShortcodes)
            {
                stringBuilder.Append(string.Format("<li>{0}</li>", shortcode.Generate(context)));
            }

            stringBuilder.Append("</ul>");

            return stringBuilder.ToString();
        }
    }
}