using System.Collections.Generic;

namespace Shortcoder
{
    // http://codex.wordpress.org/Shortcode_API#HTML
    // http://svn.automattic.com/wordpress-tests/trunk/tests/shortcode.php

    public interface IShortcodeParser
    {
        List<IShortcode> GetShortcodes(string content);

        string Parse(string content);
    }
}