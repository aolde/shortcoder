using System.Collections.Generic;

namespace Shortcoder
{
    public abstract class Shortcode : IShortcode
    {
        public string Content { get; set; }
        public Dictionary<string, object> Attributes { get; set; }

        public abstract string Generate(IShortcodeContext context);

        protected bool IsSet(string attributeName)
        {
            return Attributes.ContainsKey(attributeName);
        }

        //protected bool IsSet(Expression<Func<T, object>> propertyLambda)
        //{
        //    return IsSet(propertyLambda.Name);
        //}
    }
}
