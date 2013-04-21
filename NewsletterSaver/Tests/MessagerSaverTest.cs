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
            MessageSaver Saver = new MessageSaver(File);
            // action
            string FileName = Saver.Save(null);
            // assert
            Assert.IsNull(FileName);
        }
    }
}