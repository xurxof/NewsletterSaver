using SystemWrapper.IO;
using Moq;
using NUnit.Framework;

namespace NewsletterSaver.Tests {
    [TestFixture]
    internal sealed class InMemoryDocSaverTest {
        [Test]
        public void Save_DocuemntIsNull_DoNothingReturnFalse() {
            // arrange
            InMemoryDocSaver Saver = new InMemoryDocSaver(null);
            // action
            bool Saved = Saver.Save(null, "file.txt");
            // assert
            Assert.IsFalse(Saved);
        }

        [Test]
        public void Save_TextIsNull_DoNothingReturnFalse() {
            // arrange
            var file = new Mock<IFileWrap>(MockBehavior.Strict);
            string MainFilePath = "c:\tmp\file.txt";
            string MainFileContent = "blabla";
            string ImageFilePath = "c:\tmp\files\file.jpg";
            var ImageContent = new byte[] {5, 6};
            file.Setup(f => f.WriteAllText(MainFilePath, MainFileContent));

            file.Setup(f => f.WriteAllBytes(ImageFilePath, ImageContent));
            InMemoryDocSaver Saver = new InMemoryDocSaver(file.Object);
            InMemoryDoc doc = new InMemoryDoc(MainFileContent);
            doc.AddBinaryReference(new BinaryReference(null, ImageFilePath, ImageContent));
            // action
            bool Saved = Saver.Save(doc, MainFilePath);
            // assert
            Assert.IsTrue(Saved);
            file.Verify(f => f.WriteAllText(MainFilePath, MainFileContent), Times.Once());
            file.Verify(f => f.WriteAllBytes(ImageFilePath, ImageContent), Times.Once());
        }
    }
}