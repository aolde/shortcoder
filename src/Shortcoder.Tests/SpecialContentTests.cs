using Shortcoder.Tests.DummyShortcodes;
using Xunit;

namespace Shortcoder.Tests
{
    public class SpecialContentTests
    {
        private IShortcodeParser _shortcodeParser;

        public SpecialContentTests()
        {
            var shortcodeProvider = new ShortcodeProvider();
            shortcodeProvider.Add<DummyShortcode>();
            shortcodeProvider.Add<DummySmallShortcode>(tag: "dummy-small");
            shortcodeProvider.Add<DummyMediumShortcode>(tag: "dummy-medium");
            shortcodeProvider.Add<DummyHardShortcode>(tag: "dummy-hard");
            shortcodeProvider.Add<DummyAdvancedShortcode>(tag: "dummy-advanced");

            _shortcodeParser = new ShortcodeParser(shortcodeProvider);
        }

        [Fact]
        public void IgnorePlainText()
        {
            var content = "Lorem ipsum dolor sit amet.";

            var result = _shortcodeParser.Parse(content);

            Assert.Equal(content, result);
        }

        [Fact]
        public void IgnoreUnregisteredShortcode()
        {
            var content = "[random]";

            var result = _shortcodeParser.Parse(content);

            Assert.Equal(content, result);
        }

        [Fact]
        public void IgnoreUnregisteredShortcodeWithAttribute()
        {
            var content = "[random name=\"Joe\"]";

            var result = _shortcodeParser.Parse(content);

            Assert.Equal(content, result);
        }

        [Fact]
        public void AttributeWithBracketsShouldWork()
        {
            var content = "[dummy-medium name=\"[Joe Doe]\"]";

            var result = _shortcodeParser.Parse(content);

            Assert.Equal("Dummy Medium, and you are [Joe Doe].", result);
        }

        [Fact]
        public void SpaceShouldNotAffectParsing()
        {
            var content = "[  dummy-medium name  =  \"Hello World\"  ]";

            var result = _shortcodeParser.Parse(content);

            Assert.Equal("Dummy Medium, and you are Hello World.", result);
        }

        [Fact]
        public void MultipleShortcodesMixedWithPlainText()
        {
            var content = "Lorem ipsum dolor sit. [dummy-medium name=Hello] [dummy-medium name=Hello] [dummy-medium name=Hello World] Lorem ipsum dolor sit.";

            var result = _shortcodeParser.Parse(content);

            Assert.Equal("Lorem ipsum dolor sit. Dummy Medium, and you are Hello. Dummy Medium, and you are Hello. Dummy Medium, and you are Hello World. Lorem ipsum dolor sit.", result);
        }

        [Fact]
        public void AttributeWithHtmlShouldWork()
        {
            var content = "[dummy-medium name=\"<b>Hello World</b>\"]";

            var result = _shortcodeParser.Parse(content);

            Assert.Equal("Dummy Medium, and you are <b>Hello World</b>.", result);
        }

        [Fact]
        public void AttributeWithHtmlWithCssClassShouldWork()
        {
            var content = "[dummy-medium name=\'<span class=\"highlight\">Hello World</span>\']";

            var result = _shortcodeParser.Parse(content);

            Assert.Equal("Dummy Medium, and you are <span class=\"highlight\">Hello World</span>.", result);
        }

        [Fact]
        public void UnknownShortcodesShouldNotBeEscaped()
        {
            var content = "[[random name=\"Joe\"]]";

            var result = _shortcodeParser.Parse(content);

            Assert.Equal(content, result);
        }

        [Fact]
        public void SingleEscapedShortcodeShouldNotBeTransformed()
        {
            var content = "[[dummy name=\"Joe\"]]";

            var result = _shortcodeParser.Parse(content);

            Assert.Equal("[dummy name=\"Joe\"]", result);
        }

        [Fact]
        public void EscapedShortcodeShouldNotBeTransformed()
        {
            var content = "[[dummy name=Joe]Any HTML or shortcode may go here.[/dummy]]";

            var result = _shortcodeParser.Parse(content);

            Assert.Equal("[dummy name=Joe]Any HTML or shortcode may go here.[/dummy]", result);
        }

        [Fact]
        public void MixedEscapedAndNonEscapedTagsShouldWork()
        {
            var content1 = "[dummy-medium name=Joe] [[dummy name=Joe]Any HTML or shortcode may go here.[/dummy]] [dummy-medium name=Joe]";
            var content2 = "Lorem ipsum dolor sit amet. [dummy-medium name=Joe] [[dummy name=Joe]Any HTML or shortcode may go here.[/dummy]]";
            var content3 = "[dummy-medium name=Joe] [[dummy name=Joe]Any HTML or shortcode may go here.[/dummy]] Lorem ipsum dolor sit amet.";

            var result1 = _shortcodeParser.Parse(content1);
            var result2 = _shortcodeParser.Parse(content2);
            var result3 = _shortcodeParser.Parse(content3);

            Assert.Equal("Dummy Medium, and you are Joe. [dummy name=Joe]Any HTML or shortcode may go here.[/dummy] Dummy Medium, and you are Joe.", result1);
            Assert.Equal("Lorem ipsum dolor sit amet. Dummy Medium, and you are Joe. [dummy name=Joe]Any HTML or shortcode may go here.[/dummy]", result2);
            Assert.Equal("Dummy Medium, and you are Joe. [dummy name=Joe]Any HTML or shortcode may go here.[/dummy] Lorem ipsum dolor sit amet.", result3);
        }

        [Fact]
        public void IncompleteEscapedTagsShouldBeIgnored()
        {
            var content = "lorem [[dummy-small] lorem ipsum [dummy-small]";

            var result = _shortcodeParser.Parse(content);

            Assert.Equal("lorem [[dummy-small] lorem ipsum From Dummy Small", result);
        }

        [Theory]
        [InlineData("[ [dummy-small]", "[ From Dummy Small")]
        [InlineData("[dummy-small [dummy-small]", "[dummy-small From Dummy Small")]
        [InlineData("[dummy-small[dummy-small]", "[dummy-small[dummy-small]")]
        public void IncompleteTagsShouldBeIgnored(string content, string expected)
        {
            var result = _shortcodeParser.Parse(content);

            Assert.Equal(expected, result);
        }
    }
}
