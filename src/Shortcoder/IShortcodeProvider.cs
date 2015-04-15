using System;
using System.Collections.Generic;
using System.Reflection;

namespace Shortcoder
{
    public interface IShortcodeProvider
    {
        void Add<T>(string tag = null);
        void Add(Type shortcodeType, string tag = null);
        void AddFromAssembly(Assembly assembly);
        void Remove(string tag);
        bool Exists(string tag);
        Type Get(string tag);
        IShortcode Create(string tag, Dictionary<string, object> attributes = null, string content = null);
    }
}