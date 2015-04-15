using System.Collections.Generic;

namespace Shortcoder
{
    public interface IShortcode
    {
        string Content { get; set; }
        Dictionary<string, object> Attributes { get; set; }

        string Generate(IShortcodeContext context);
    }
}
