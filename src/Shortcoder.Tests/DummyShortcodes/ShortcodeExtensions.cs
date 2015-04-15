using System;
using System.Linq.Expressions;
using Shortcoder.Utilities;

namespace Shortcoder.Tests.DummyShortcodes
{
    public static class ShortcodeExtensions
    {
        public static bool IsSet<T>(this T shortcode, Expression<Func<T, object>> expression) where T : IShortcode
        {
            var memberName = StaticReflection.GetMemberName(expression);

            return shortcode.Attributes.ContainsKey(memberName);
        }
    }
}