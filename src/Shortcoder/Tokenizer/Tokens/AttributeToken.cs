namespace Shortcoder.Tokenizer.Tokens
{
    public class AttributeToken : Token
    {
        public string AttributeName { get; set; }
        public object AttributeValue { get; set; }
    }
}