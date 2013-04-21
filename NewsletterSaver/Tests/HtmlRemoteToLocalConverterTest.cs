using System.Linq;
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
            Assert.AreEqual(0, MemoryDoc.BinaryReferences.Count());
        }


        [Test]
        public void Convert_NoLocableLinks_ReturnsNoLocableReferences() {
            // arrange
            HtmlRemoteToLocalConverter Converter = new HtmlRemoteToLocalConverter();
            string SimpleHtml = @"<!DOCTYPE html><html><body><h1>My First Heading</h1><p>My first paragraph.</p></body></html>";
            // action
            var MemoryDoc = Converter.Convert(SimpleHtml);
            // assert
            Assert.AreEqual(SimpleHtml,MemoryDoc.Text);
            Assert.AreEqual(0, MemoryDoc.BinaryReferences.Count());
        }
	  
    }
}
