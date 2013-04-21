using Moq;
using NUnit.Framework;

namespace NewsletterSaver.Tests {
    [TestFixture]
    internal sealed class HtmlLocalSaverTest {
        [Test]
        public void Save_TextIsNull_DoNothingReturnFalse() {
            // arrange
            HtmlLocalSaver Saver = new HtmlLocalSaver(null);
            // action
            bool Saved = Saver.Save(null, "file.txt");
            // assert
            Assert.IsFalse(Saved);
        }

        
    }
}