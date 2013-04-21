using SystemWrapper.IO;
using Moq;
using NUnit.Framework;

namespace NewsletterSaver.Tests {
    [TestFixture]
    internal sealed class MessagerSaverTest {
        [Test]
        public void Save_NotMailPasessed_DoNothingReturnNull() {
            // arrange
            var File = new Mock<IFileWrap>(MockBehavior.Strict); // avoid method calls of file 
            MessageSaver Saver = new MessageSaver(File.Object);
            // action
            string FileName = Saver.Save(null);
            // assert
            Assert.IsNull(FileName);
        }


        [Test]
        public void Save_TextMail_SaveText() {
            // arrange
            string Titulo = @"titulo:\";
            string Content = "blablabla";
            var File = new Mock<IFileWrap>();

            MessageSaver Saver = new MessageSaver(File.Object, @"C:\", new PathWrap());
            var TextMessageStub = new Mock<IStructuredMessage>();

            TextMessageStub.Setup(m => m.Title).Returns(Titulo);

            TextMessageStub.Setup(m => m.Text).Returns(Content);
            TextMessageStub.Setup(m => m.Content).Returns(ContentsTypes.Text);
            // action
            string FileName = Saver.Save(TextMessageStub.Object);
            // assert
            string Filepath = @"C:\titulo__.txt";
            Assert.AreEqual(Filepath, FileName);
            File.Verify(f => f.WriteAllText(Filepath, Content), Times.Once());
        }
    }
}