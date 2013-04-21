using NUnit.Framework;

namespace NewsletterSaver.Tests {
    [TestFixture]
    sealed class HtmlRemoteToLocalConverterTest {

        [Test]
        public void Convert_NullText_ReturnsEmptyDoc() {
            // arrange
            HtmlRemoteToLocalConverter Converter = new HtmlRemoteToLocalConverter();
            // action
            var MemoryDoc = Converter.Convert(null);
            // assert
            Assert.IsNull(MemoryDoc.Text);
        }
	  
    }
}
