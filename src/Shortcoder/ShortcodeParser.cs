using System.Collections.Generic;
using System.Linq;
using System.Text;
using Shortcoder.Parsing;
using Shortcoder.Parsing.States;

namespace Shortcoder
{
    /// <summary>
    /// 1. [gallery]
    /// 1. [gallery /]
    /// 2. [myshortcode foo="bar" bar="bing"]
    /// 2. [gallery id="123" size="medium"]
    /// 3. [caption]My Caption[/caption]
    /// 4. [caption]Caption: [myshortcode][/caption] // myshortcode won't be parsed
    /// 5. [myshortcode example='non-enclosing' /] non-enclosed content [myshortcode] enclosed content [/myshortcode]
    /// 6. [myshortcode foo='123' bar=456] is equivalent to [myshortcode foo="123" bar="456"]
    /// 7. [myshortcode 123 hello] 
    /// </summary>
    public class ShortcodeParser : IShortcodeParser
    {
        public IShortcodeProvider ShortcodeProvider { get; set; }
        public IParserState ParserState { get; set; }
        public TextParser TextParser { get; set; }
        public List<ShortcodeParseInfo> ParseInstructions { get; set; }
        public ShortcodeParseInfo CurrentShortcode { get; set; }

        public static IShortcodeParser Current
        {
            get
            {
                return ShortcodeConfiguration.ParserFactory.Create();
            }
        }

        public ShortcodeParser(IShortcodeProvider shortcodeProvider)
        {
            ShortcodeProvider = shortcodeProvider;
        }

        public string Parse(string content)
        {
            TextParser = new TextParser(content);
            ParserState = new LookingForTagState(this);
            ParseInstructions = new List<ShortcodeParseInfo>();

            while (!TextParser.EndOfText)
            {
                ParserState.Parse();
            }

            var contentBuilder = new StringBuilder(content);

            if (ParseInstructions.Any())
            {
                var contentPositionAdjust = 0;
                var initialContentLength = contentBuilder.Length;

                foreach (var shortcodeInfo in ParseInstructions.OrderBy(pi => pi.BeginPosition))
                {
                    string generatedText = null;
                    
                    if (shortcodeInfo.Tag != null)
                    {
                        var shortcode = ShortcodeProvider.Create(shortcodeInfo.Tag, shortcodeInfo.Attributes, shortcodeInfo.Content);
                        generatedText = shortcode.Generate(new ShortcodeContext { Tag = shortcodeInfo.Tag, Parser = this });
                    }
                    else
                    {
                        generatedText = shortcodeInfo.PreCompiledContent;
                    }

                    if (generatedText == null)
                    {
                        generatedText = string.Empty;
                    }

                    var startIndex = shortcodeInfo.BeginPosition + contentPositionAdjust;
                    if (startIndex < 0)
                    {
                        startIndex = 0;
                    }

                    contentBuilder.Remove(startIndex, shortcodeInfo.EndPosition - shortcodeInfo.BeginPosition);
                    contentBuilder.Insert(startIndex, generatedText);

                    contentPositionAdjust = contentBuilder.Length - initialContentLength;
                }
            }

            return contentBuilder.ToString();
        }
    }
}