using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shortcoder
{
    public class ShortcodeProvider : IShortcodeProvider
    {
        private Dictionary<string, Type> _shortcodes = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

        public void Add<T>(string tag = null)
        {
            Add(typeof(T), tag);
        }

        public void Add(Type shortcodeType, string tag = null)
        {
            if (string.IsNullOrEmpty(tag))
            {
                tag = GetTagFromType(shortcodeType);
            }

            if (_shortcodes.ContainsKey(tag))
                throw new InvalidOperationException("A shortcode named " + tag + " has already been added.");

            _shortcodes.Add(tag, shortcodeType);
        }

        public void AddFromAssembly(Assembly assembly)
        {
            var type = typeof(IShortcode);
            var types = assembly.GetTypes()
                .Where(t => type.IsAssignableFrom(t));

            foreach (var shortcodeType in types)
            {
                var tag = GetTagFromType(shortcodeType);

                if (!Exists(tag))
                {
                    Add(shortcodeType);
                }
            }
        }

        public void Remove(string tag)
        {
            _shortcodes.Remove(tag);
        }

        public bool Exists(string tag)
        {
            return _shortcodes.ContainsKey(tag);
        }

        public Type Get(string tag)
        {
            return _shortcodes[tag];
        }

        public IShortcode Create(string tag, Dictionary<string, object> attributes = null, string content = null)
        {
            var type = Get(tag);
            var shortcode = Activator.CreateInstance(type) as IShortcode;

            if (shortcode != null)
            {
                shortcode.Attributes = attributes;
                shortcode.Content = content;

                BindModel(shortcode);
            }

            return shortcode;
        }

        private void BindModel(IShortcode shortcode)
        {
            if (shortcode.Attributes != null)
            {
                var properties = shortcode.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var attribute in shortcode.Attributes)
                {
                    var propertyInfo = properties.FirstOrDefault(p => p.Name.Equals(attribute.Key, StringComparison.InvariantCultureIgnoreCase));

                    if (propertyInfo != null)
                    {
                        var type = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                        var safeValue = attribute.Value == null ? null : Convert.ChangeType(attribute.Value, type);

                        propertyInfo.SetValue(shortcode, safeValue, null);
                    }
                }
            }
        }

        private string GetTagFromType(Type shortcodeType)
        {
            return shortcodeType.Name.ToLower().Replace("shortcode", string.Empty);
        }
    }
}