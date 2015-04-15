using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shortcoder.Tests.DummyShortcodes;

namespace Shortcoder.Tests
{
    [TestClass]
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

        [TestMethod]
        public void IgnorePlainText()
        {
            var content = "Lorem ipsum dolor sit amet.";

            var result = _shortcodeParser.Parse(content);

            Assert.AreEqual(content, result);
        }

        [TestMethod]
        public void IgnoreUnregisteredShortcode()
        {
            var content = "[random]";

            var result = _shortcodeParser.Parse(content);

            Assert.AreEqual(content, result);
        }

        [TestMethod]
        public void IgnoreUnregisteredShortcodeWithAttribute()
        {
            var content = "[random name=\"Joe\"]";

            var result = _shortcodeParser.Parse(content);

            Assert.AreEqual(content, result);
        }

        [TestMethod]
        public void AttributeWithBracketsShouldWork()
        {
            var content = "[dummy-medium name=\"[Joe Doe]\"]";

            var result = _shortcodeParser.Parse(content);

            Assert.AreEqual("Dummy Medium, and you are [Joe Doe].", result);
        }

        [TestMethod]
        public void SpaceShouldNotAffectParsing()
        {
            var content = "[  dummy-medium name  =  \"Hello World\"  ]";

            var result = _shortcodeParser.Parse(content);

            Assert.AreEqual("Dummy Medium, and you are Hello World.", result);
        }

        [TestMethod]
        public void MultipleShortcodesMixedWithPlainText()
        {
            var content = "Lorem ipsum dolor sit. [dummy-medium name=Hello] [dummy-medium name=Hello] [dummy-medium name=Hello World] Lorem ipsum dolor sit.";

            var result = _shortcodeParser.Parse(content);

            Assert.AreEqual("Lorem ipsum dolor sit. Dummy Medium, and you are Hello. Dummy Medium, and you are Hello. Dummy Medium, and you are Hello World. Lorem ipsum dolor sit.", result);
        }

        [TestMethod]
        public void AttributeWithHtmlShouldWork()
        {
            var content = "[dummy-medium name=\"<b>Hello World</b>\"]";

            var result = _shortcodeParser.Parse(content);

            Assert.AreEqual("Dummy Medium, and you are <b>Hello World</b>.", result);
        }

        [TestMethod]
        public void AttributeWithHtmlWithCssClassShouldWork()
        {
            var content = "[dummy-medium name=\'<span class=\"highlight\">Hello World</span>\']";

            var result = _shortcodeParser.Parse(content);

            Assert.AreEqual("Dummy Medium, and you are <span class=\"highlight\">Hello World</span>.", result);
        }

        [TestMethod]
        public void UnknownShortcodesShouldNotBeEscaped()
        {
            var content = "[[random name=\"Joe\"]]";

            var result = _shortcodeParser.Parse(content);

            Assert.AreEqual(content, result);
        }

        [TestMethod]
        public void SingleEscapedShortcodeShouldNotBeTransformed()
        {
            var content = "[[dummy name=\"Joe\"]]";

            var result = _shortcodeParser.Parse(content);

            Assert.AreEqual("[dummy name=\"Joe\"]", result);
        }

        [TestMethod]
        public void EscapedShortcodeShouldNotBeTransformed()
        {
            var content = "[[dummy name=Joe]Any HTML or shortcode may go here.[/dummy]]";

            var result = _shortcodeParser.Parse(content);

            Assert.AreEqual("[dummy name=Joe]Any HTML or shortcode may go here.[/dummy]", result);
        }

        [TestMethod]
        public void MixedEscapedAndNonEscapedTagsShouldWork()
        {
            var content1 = "[dummy-medium name=Joe] [[dummy name=Joe]Any HTML or shortcode may go here.[/dummy]] [dummy-medium name=Joe]";
            var content2 = "Lorem ipsum dolor sit amet. [dummy-medium name=Joe] [[dummy name=Joe]Any HTML or shortcode may go here.[/dummy]]";
            var content3 = "[dummy-medium name=Joe] [[dummy name=Joe]Any HTML or shortcode may go here.[/dummy]] Lorem ipsum dolor sit amet.";

            var result1 = _shortcodeParser.Parse(content1);
            var result2 = _shortcodeParser.Parse(content2);
            var result3 = _shortcodeParser.Parse(content3);

            Assert.AreEqual("Dummy Medium, and you are Joe. [dummy name=Joe]Any HTML or shortcode may go here.[/dummy] Dummy Medium, and you are Joe.", result1);
            Assert.AreEqual("Lorem ipsum dolor sit amet. Dummy Medium, and you are Joe. [dummy name=Joe]Any HTML or shortcode may go here.[/dummy]", result2);
            Assert.AreEqual("Dummy Medium, and you are Joe. [dummy name=Joe]Any HTML or shortcode may go here.[/dummy] Lorem ipsum dolor sit amet.", result3);
        }

        [TestMethod]
        public void IncompleteEscapedTagsShouldBeIgnored()
        {
            var content = "lorem [[dummy-small] lorem ipsum [dummy-small]";

            var result = _shortcodeParser.Parse(content);

            Assert.AreEqual("lorem [[dummy-small] lorem ipsum From Dummy Small", result);
        }

        [TestMethod]
        public void IncompleteTagsShouldBeIgnored()
        {
            var content1 = "[ [dummy-small]";
            var content2 = "[dummy-small [dummy-small]";
            var content3 = "[dummy-small[dummy-small]";

            var result1 = _shortcodeParser.Parse(content1);
            var result2 = _shortcodeParser.Parse(content2);
            var result3 = _shortcodeParser.Parse(content3);

            Assert.AreEqual("[ From Dummy Small", result1);
            Assert.AreEqual("[dummy-small From Dummy Small", result2);
            Assert.AreEqual("[dummy-small[dummy-small]", result3);
        }
    }
}
