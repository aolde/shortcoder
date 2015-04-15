namespace Shortcoder.Parsing.States
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
    public class ParsingAttributesState : ParserState
    {
        public ParsingAttributesState(ShortcodeParser shortcodeParser) : base(shortcodeParser) {}

        public override void Parse()
        {
            var beginNamePosition = _textParser.Position;

            _textParser.MoveTo(new[] { SPACE_CHAR, EQUAL_CHAR, TAG_END_CHAR });

            var attributeName = _textParser.Extract(beginNamePosition, _textParser.Position);

            AddAttribute(attributeName);

            _textParser.MovePastWhitespace();

            if (_textParser.Peek() == EQUAL_CHAR) // attribute has a value
            {
                _textParser.MoveAhead();
                _textParser.MovePastWhitespace();

                var peek = _textParser.Peek();
                char enclosingChar = SPACE_CHAR;
                bool isInQuotation = false;

                if (peek == SINGLE_QUOTE_CHAR || peek == DOUBLE_QUOTE_CHAR)
                {
                    isInQuotation = true;
                    enclosingChar = peek;

                    _textParser.MoveAhead();
                }

                var beginValuePosition = _textParser.Position;

                _textParser.MoveTo(isInQuotation ? new[] { enclosingChar } : new[] { TAG_END_CHAR, CLOSING_SLASH_CHAR });

                var attributeValue = _textParser.Extract(beginValuePosition, _textParser.Position);
                
                _shortcodeParser.CurrentShortcode.Attributes[attributeName] = attributeValue;

                _textParser.MovePast(new[] { enclosingChar, CLOSING_SLASH_CHAR });
            }

            _shortcodeParser.CurrentShortcode.EndPosition = _textParser.Position + 1;
            
            if (_textParser.Peek() == SPACE_CHAR)
            {
                _textParser.MovePast(new[] { SPACE_CHAR });
                SetState(new ParsingAttributesState(_shortcodeParser));
            }
            else
            {
                StoreCurrentShortcode();

                SetState(new LookingForTagState(_shortcodeParser));  
            }
        }

        private void AddAttribute(string attributeName)
        {
            if (!_shortcodeParser.CurrentShortcode.Attributes.ContainsKey(attributeName))
            {
                _shortcodeParser.CurrentShortcode.Attributes.Add(attributeName, string.Empty);
            }
        }
    }
}