namespace Shortcoder.Tokenizer.Tokens
{
    /// <summary>
    /// Matching "[tagname " or "[tagname]".
    /// </summary>
    public class TagStartToken : Token
    {
        public string TagName { get; set; }
        public bool IsSelfClosing { get; set; }
    }
}