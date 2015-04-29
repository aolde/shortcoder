using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shortcoder.Tests.DummyShortcodes;

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

    [TestClass]
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

            _shortcodeParser = new ShortcodeParser(shortcodeProvider);    
        }

        [TestMethod]
        public void DummyShortcodeShouldReturnNull()
        {
            var content = "[dummy]";

            var result = _shortcodeParser.Parse(content);

            Assert.AreEqual(string.Empty, result);
        }

        [TestMethod]
        public void DummySmallShouldReturnGeneratedText()
        {
            var content = "Hello World [dummy-small]";

            var result = _shortcodeParser.Parse(content);

            Assert.AreEqual("Hello World From Dummy Small", result);
        }

        [TestMethod]
        public void MultipleDummySmallShouldReturnGeneratedText()
        {
            var content = "Hello World [dummy-small] and [dummy-small]";

            var result = _shortcodeParser.Parse(content);

            Assert.AreEqual("Hello World From Dummy Small and From Dummy Small", result);
        }

        [TestMethod]
        public void DummySmallShouldReturnTextUsingClosingTag()
        {
            var content1 = "Hello World [dummy-small /]";
            var content2 = "Hello World [dummy-small/]";

            var result1 = _shortcodeParser.Parse(content1);
            var result2 = _shortcodeParser.Parse(content2);

            var expected = "Hello World From Dummy Small";

            Assert.AreEqual(expected, result1);
            Assert.AreEqual(expected, result2);
        }

        [TestMethod]
        public void DummyMediumShouldReturnGeneratedTextUsingDoubleQuoteAttribute()
        {
            var content = "[dummy-medium name=\"Joe\"]";

            var result = _shortcodeParser.Parse(content);

            Assert.AreEqual("Dummy Medium, and you are Joe.", result);
        }

        [TestMethod]
        public void DummyMediumShouldReturnGeneratedTextUsingMultipleAttributes()
        {
            var content = "[dummy-medium name=\"Joe\" age=\"12\"]";

            var result = _shortcodeParser.Parse(content);

            Assert.AreEqual("Dummy Medium, and you are Joe, age 12.", result);
        }

        [TestMethod]
        public void DummyMediumShouldReturnGeneratedTextUsingSingleQuoteAttribute()
        {
            var content = "[dummy-medium name='Joe']";

            var result = _shortcodeParser.Parse(content);

            Assert.AreEqual("Dummy Medium, and you are Joe.", result);
        }

        [TestMethod]
        public void DummyMediumShouldReturnGeneratedTextUsingAttributeWithoutQuotes()
        {
            var content = "[dummy-medium name=Joe]";

            var result = _shortcodeParser.Parse(content);

            Assert.AreEqual("Dummy Medium, and you are Joe.", result);
        }

        [TestMethod]
        public void DummyMediumShouldReturnGeneratedTextUsingAttributeWithoutValue()
        {
            var content = "[dummy-medium name]";

            var result = _shortcodeParser.Parse(content);

            Assert.AreEqual("Dummy Medium, and you are .", result);
        }

        [TestMethod]
        public void DummyHardShouldReturnGeneratedTextUsingPropertyGetters()
        {
            var content = "[dummy-hard name=\"Joe\" age=12]";

            var result = _shortcodeParser.Parse(content);

            Assert.AreEqual("Dummy Hard, and you are Joe, age 12.", result);
        }

        [TestMethod]
        public void DummyAdvancedShouldReturnGeneratedTextUsingPropertyGetters()
        {
            var content = "[dummy-advanced name=\"Joe\" age=20]Dummy Advanced[/dummy-advanced]. " +
                          "[dummy-advanced name=\"Doe\" age=30]Dummy 2 Advanced[/dummy-advanced]";

            var result = _shortcodeParser.Parse(content);

            Assert.AreEqual("Hello Joe, 20 (Dummy Advanced). Hello Doe, 30 (Dummy 2 Advanced)", result);
        }

        [TestMethod]
        public void DummyAdvancdContentWithChildShortcodesShouldNotBeParsed()
        {
            var content1 = "[dummy-advanced name=\"Joe\" age=20]Hello [dummy-medium name=\"Doe\"] World[/dummy-advanced]";
            var content2 = "[dummy-advanced name=\"Joe\" age='20' /]Hello [dummy-advanced name=\"Doe\"] World[/dummy-advanced]";
            var content3 = "[dummy-advanced name=\"Joe\" age=20]Hello [dummy-advanced name=\"Doe\"]Big[/dummy-advanced] World[/dummy-advanced]";

            var result1 = _shortcodeParser.Parse(content1);
            var result2 = _shortcodeParser.Parse(content2);
            var result3 = _shortcodeParser.Parse(content3);

            Assert.AreEqual("Hello Joe, 20 (Hello [dummy-medium name=\"Doe\"] World)", result1);
            Assert.AreEqual("Hello Joe, 20 (Hello [dummy-advanced name=\"Doe\"] World)", result2);
            Assert.AreEqual("Hello Joe, 20 (Hello [dummy-advanced name=\"Doe\"]Big[/dummy-advanced] World)", result3);
        }

    }
}
