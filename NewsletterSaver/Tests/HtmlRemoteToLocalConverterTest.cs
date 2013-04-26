using System.Linq;
using Moq;
using NUnit.Framework;

namespace NewsletterSaver.Tests {
    [TestFixture]
    sealed class HtmlRemoteToLocalConverterTest {

        [Test]
        public void Convert_NullText_ReturnsEmptyDoc() {
            // arrange
            HtmlRemoteToLocalConverter Converter = new HtmlRemoteToLocalConverter(null);
            // action
            var MemoryDoc = Converter.Convert(null, "",@"C:\tmp","title");
            // assert
            Assert.IsNull(MemoryDoc.Text);
            Assert.AreEqual(0, MemoryDoc.BinaryReferences.Count());
        }


        [Test]
        public void Convert_NoLocableLinks_ReturnsNoLocableReferences() {
            // arrange
            HtmlRemoteToLocalConverter Converter = new HtmlRemoteToLocalConverter(null);
            string SimpleHtml = @"<!DOCTYPE html><html><body><h1>My First Heading</h1><p>My first paragraph.</p></body></html>";
            // action
            var MemoryDoc = Converter.Convert(SimpleHtml, "", @"C:\tmp", "title");
            // assert
            Assert.AreEqual(SimpleHtml,MemoryDoc.Text);
            Assert.AreEqual(0, MemoryDoc.BinaryReferences.Count());
        }

        [Test]
        [Ignore]
        public void Convert_RemoteImagesLinks_ReturnsBinaryReferences() {
            Mock<IWebFacade> Web = new Mock<IWebFacade>();

            HtmlRemoteToLocalConverter Converter = new HtmlRemoteToLocalConverter(Web.Object);
            byte[] Image = new byte[] { 3, 2 };
            Web.Setup(w => w.GetBinaryRemoteFile("w3schools.jpg")).Returns(Image);
            string SimpleHtml = @"<!DOCTYPE html><html><body><h1>My First Heading</h1><p>My first paragraph.</p><img src=""w3schools.jpg"" width=""104"" height=""142""></body></body></html>";
            
            // action
            var MemoryDoc = Converter.Convert(SimpleHtml, "", @"C:\tmp", "title");
            // assert
            Assert.AreEqual(SimpleHtml, MemoryDoc.Text);
            Assert.AreEqual(1, MemoryDoc.BinaryReferences.Count());
            Assert.AreEqual(@"C:\tmp\w3schools.jpg", MemoryDoc.BinaryReferences.First().NewLocalLink);
            Assert.AreEqual("w3schools.jpg", MemoryDoc.BinaryReferences.First().OriginalLink);
            Assert.AreSame(Image, MemoryDoc.BinaryReferences.First().BinaryValue);
        }
        [Test]
        [Ignore]
        public void Convert_RemoteImagesLinksDuplicated_ReturnsNotDuplicatedBinaryReferences() {
            Mock<IWebFacade> Web = new Mock<IWebFacade>();

            HtmlRemoteToLocalConverter Converter = new HtmlRemoteToLocalConverter(Web.Object);
            byte[] Image = new byte[] { 3, 2 };
            Web.Setup(w => w.GetBinaryRemoteFile("w3schools.jpg")).Returns(Image);
            string SimpleHtml = @"<!DOCTYPE html><html><body><h1>My First Heading</h1><p>My first paragraph.</p><img src=""w3schools.jpg"" width=""104"" height=""142""><img src=""w3schools.jpg"" width=""104"" height=""142""></body></body></html>";

            // action
            var MemoryDoc = Converter.Convert(SimpleHtml, "", @"C:\tmp","title");
            // assert
            Assert.AreEqual(SimpleHtml, MemoryDoc.Text);
            Assert.AreEqual(1, MemoryDoc.BinaryReferences.Count());
            Assert.AreEqual(@"C:\tmp\w3schools.jpg", MemoryDoc.BinaryReferences.First().NewLocalLink);
            Assert.AreEqual("w3schools.jpg", MemoryDoc.BinaryReferences.First().OriginalLink);
            Assert.AreSame(Image, MemoryDoc.BinaryReferences.First().BinaryValue);
        }
    }
}
