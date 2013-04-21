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
            var File = new Mock<IFileWrap>(MockBehavior.Strict);
            string MainFilePath = "c:\tmp\file.txt";
            string MainFileContent = "blabla";
            string ImageFilePath = "c:\tmp\files\file.jpg";
            var ImageContent = new byte[] {5, 6};
            File.Setup(f => f.WriteAllText(MainFilePath, MainFileContent));

            File.Setup(f => f.WriteAllBytes(ImageFilePath, ImageContent));
            InMemoryDocSaver Saver = new InMemoryDocSaver(File.Object);
            InMemoryDoc Doc = new InMemoryDoc(MainFileContent);
            Doc.AddBinaryReference(new BinaryReference(null, ImageFilePath, ImageContent));
            // action
            bool Saved = Saver.Save(Doc, MainFilePath);
            // assert
            Assert.IsTrue(Saved);
            File.Verify(f => f.WriteAllText(MainFilePath, MainFileContent), Times.Once());
            File.Verify(f => f.WriteAllBytes(ImageFilePath, ImageContent), Times.Once());
        }
    }
}