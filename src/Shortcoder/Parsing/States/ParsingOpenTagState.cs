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
    public class ParsingOpenTagState : ParserState
    {
        public ParsingOpenTagState(ShortcodeParser shortcodeParser)
            : base(shortcodeParser)
        {
        }

        /// <summary>
        /// Gets or sets position where the tag starts.
        /// </summary>
        public int TagBeginPosition { get; set; }

        public override void Parse()
        {
            var tagNameBeginPosition = _textParser.Position;

            _textParser.MoveTo(new[] { SPACE_CHAR, CLOSING_SLASH_CHAR, TAG_END_CHAR });

            var tagName = _textParser.Extract(tagNameBeginPosition, _textParser.Position);

            if (_shortcodeParser.ShortcodeProvider.Exists(tagName))
            {
                _shortcodeParser.CurrentShortcode = new ShortcodeParseInfo
                {
                    Tag = tagName,
                    BeginPosition = TagBeginPosition,
                    EndPosition = _textParser.Position + 1
                };

                _textParser.MovePastWhitespace();

                var peek = _textParser.Peek();

                if (peek == TAG_END_CHAR) // end char for tag
                {
                    SetState(new LookingForTagState(_shortcodeParser));
                }
                else if (peek == CLOSING_SLASH_CHAR) // closing tag
                {
                    _textParser.MovePast(new[] { TAG_BEGIN_CHAR, CLOSING_SLASH_CHAR });

                    _shortcodeParser.CurrentShortcode.IsClosed = true;
                    _shortcodeParser.CurrentShortcode.EndPosition = _textParser.Position + 1;

                    StoreCurrentShortcode();

                    SetState(new LookingForTagState(_shortcodeParser));
                }
                else if (IsAttributeNameChar(peek)) // is start of an attribute
                {
                    SetState(new ParsingAttributesState(_shortcodeParser));
                }
                else
                {
                    // invalid tag, reset currentShortcode and start looking for new tag.
                    _shortcodeParser.CurrentShortcode = null;
                    SetState(new LookingForTagState(_shortcodeParser));
                }
            }
            else
            {
                _textParser.MoveTo(TAG_END_CHAR);
                SetState(new LookingForTagState(_shortcodeParser));
            }
        }
    }
}