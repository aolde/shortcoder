using System;
using Shortcoder.Tests.DummyShortcodes;
using Xunit;

namespace Shortcoder.Tests
{
    /// 1. [gallery]
    /// 1. [gallery /]
    /// 2. [myshortcode foo="bar" bar="bing"]
    /// 2. [gallery id="123" size="medium"]
    /// 3. [caption]My Caption[/caption]
    /// 4. [caption]Caption: [myshortcode][/caption] // myshortcode won't be parsed
    /// 5. [myshortcode example='non-enclosing' /] non-enclosed content [myshortcode] enclosed content [/myshortcode]
    /// 6. [myshortcode foo='123' bar=456] is equivalent to [myshortcode foo="123" bar="456"]
    /// 7. [myshortcode 123 hello] 
    public class DummyShortcodeTests
    {
        private IShortcodeParser _shortcodeParser;

        public DummyShortcodeTests()
        {
            var shortcodeProvider = new ShortcodeProvider();
            shortcodeProvider.Add<DummyShortcode>();
            shortcodeProvider.Add<DummySmallShortcode>(tag:"dummy-small");
            shortcodeProvider.Add<DummyMediumShortcode>(tag: "dummy-medium");
            shortcodeProvider.Add<DummyHardShortcode>(tag: "dummy-hard");
            shortcodeProvider.Add<DummyAdvancedShortcode>(tag: "dummy-advanced");
            shortcodeProvider.Add<DummyListShortcode>(tag: "dummy-list");
            shortcodeProvider.Add<DummyListItemShortcode>(tag: "dummy-list-item");

            _shortcodeParser = new ShortcodeParser(shortcodeProvider);    
        }

        [Fact]
        public void DummyShortcodeShouldReturnNull()
        {
            var content = "[dummy]";

            var result = _shortcodeParser.Parse(content);

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void DummySmallShouldReturnGeneratedText()
        {
            var content = "Hello World [dummy-small]";

            var result = _shortcodeParser.Parse(content);

            Assert.Equal("Hello World From Dummy Small", result);
        }

        [Fact]
        public void MultipleDummySmallShouldReturnGeneratedText()
        {
            var content = "Hello World [dummy-small] and [dummy-small]";

            var result = _shortcodeParser.Parse(content);

            Assert.Equal("Hello World From Dummy Small and From Dummy Small", result);
        }

        [Fact]
        public void DummySmallShouldReturnTextUsingClosingTag()
        {
            var content1 = "Hello World [dummy-small /]";
            var content2 = "Hello World [dummy-small/]";

            var result1 = _shortcodeParser.Parse(content1);
            var result2 = _shortcodeParser.Parse(content2);

            var expected = "Hello World From Dummy Small";

            Assert.Equal(expected, result1);
            Assert.Equal(expected, result2);
        }

        [Fact]
        public void DummyMediumShouldReturnGeneratedTextUsingDoubleQuoteAttribute()
        {
            var content = "[dummy-medium name=\"Joe\"]";

            var result = _shortcodeParser.Parse(content);

            Assert.Equal("Dummy Medium, and you are Joe.", result);
        }

        [Fact]
        public void DummyMediumShouldReturnGeneratedTextUsingMultipleAttributes()
        {
            var content = "[dummy-medium name=\"Joe\" age=\"12\"]";

            var result = _shortcodeParser.Parse(content);

            Assert.Equal("Dummy Medium, and you are Joe, age 12.", result);
        }

        [Fact]
        public void DummyMediumShouldReturnGeneratedTextUsingSingleQuoteAttribute()
        {
            var content = "[dummy-medium name='Joe']";

            var result = _shortcodeParser.Parse(content);

            Assert.Equal("Dummy Medium, and you are Joe.", result);
        }

        [Fact]
        public void DummyMediumShouldReturnGeneratedTextUsingAttributeWithoutQuotes()
        {
            var content = "[dummy-medium name=Joe]";

            var result = _shortcodeParser.Parse(content);

            Assert.Equal("Dummy Medium, and you are Joe.", result);
        }

        [Fact]
        public void DummyMediumShouldReturnGeneratedTextUsingAttributeWithoutValue()
        {
            var content = "[dummy-medium name]";

            var result = _shortcodeParser.Parse(content);

            Assert.Equal("Dummy Medium, and you are .", result);
        }

        [Fact]
        public void DummyHardShouldReturnGeneratedTextUsingPropertyGetters()
        {
            var content = "[dummy-hard name=\"Joe\" age=12]";

            var result = _shortcodeParser.Parse(content);

            Assert.Equal("Dummy Hard, and you are Joe, age 12.", result);
        }

        [Fact]
        public void DummyAdvancedShouldReturnGeneratedTextUsingPropertyGetters()
        {
            var content = "[dummy-advanced name=\"Joe\" age=20]Dummy Advanced[/dummy-advanced]. " +
                          "[dummy-advanced name=\"Doe\" age=30]Dummy 2 Advanced[/dummy-advanced]";

            var result = _shortcodeParser.Parse(content);

            Assert.Equal("Hello Joe, 20 (Dummy Advanced). Hello Doe, 30 (Dummy 2 Advanced)", result);
        }

        [Fact]
        public void DummyAdvancdContentWithChildShortcodesShouldNotBeParsed()
        {
            var content1 = "[dummy-advanced name=\"Joe\" age=20]Hello [dummy-medium name=\"Doe\"] World[/dummy-advanced]";
            var content2 = "[dummy-advanced name=\"Joe\" age='20' /] and [dummy-advanced name=\"Doe\" age=22]World[/dummy-advanced]";
            var content3 = "[dummy-advanced name=\"Joe\" age=20]Hello [dummy-advanced name=\"Doe\"]Big[/dummy-advanced] World[/dummy-advanced]";

            var result1 = _shortcodeParser.Parse(content1);
            var result2 = _shortcodeParser.Parse(content2);
            var result3 = _shortcodeParser.Parse(content3);

            Assert.Equal("Hello Joe, 20 (Hello [dummy-medium name=\"Doe\"] World)", result1);
            Assert.Equal("Hello Joe, 20 and Hello Doe, 22 (World)", result2);
            Assert.Equal("Hello Joe, 20 (Hello [dummy-advanced name=\"Doe\"]Big[/dummy-advanced] World)", result3);
        }

        [Fact]
        public void DummyAdvancedShouldFailNicelyWithWrongTypeAttribute()
        {
            var content1 = "[dummy-advanced age='hello' /]";

            Assert.Throws<FormatException>(() => _shortcodeParser.Parse(content1));
        }

        [Fact]
        public void DummyAdvancedShouldReadAttributesWithSlash()
        {
            var content1 = "[dummy-advanced name='/slash' age=3 /]";

            var result1 = _shortcodeParser.Parse(content1);

            Assert.Equal("Hello /slash, 3", result1);
        }

        [Theory]
        //[InlineData("[dummy-list]", "")]
        //[InlineData("[dummy-list][dummy-list-item][/dummy-list]", "<ul><li>Item</li></ul>")]
        //[InlineData("[dummy-list][dummy-list-item /][/dummy-list]", "<ul><li>Item</li></ul>")]
        //[InlineData("[dummy-list] LOREM [dummy-list-item /] LOREM [/dummy-list]", "<ul><li>Item</li></ul>")]
        //[InlineData("[dummy-list][dummy-list-item][dummy-list-item][dummy-list-item][/dummy-list]", "<ul><li>Item</li><li>Item</li><li>Item</li></ul>")]
        //[InlineData("[dummy-list][dummy-list-item][/dummy-list-item][dummy-list-item][/dummy-list]", "<ul><li>Item</li><li>Item</li></ul>")]
        //[InlineData("[dummy-list][dummy-list-item]Content[/dummy-list-item][/dummy-list]", "<ul><li>ItemContent</li></ul>")]
        //[InlineData("[dummy-list][dummy-list-item]Content[/dummy-list-item][/dummy-list]", "<ul><li>ItemContent</li></ul>")]
        [InlineData("[dummy-list][undefined]Content[/undefined][/dummy-list]", "<ul><li>Content</li></ul>")]
        public void ListTests(string content, string expected)
        {
            var result = _shortcodeParser.Parse(content);

            Assert.Equal(expected, result);
        }
    }
}
