using System;
using SystemWrapper.IO;
using Moq;
using NUnit.Framework;

namespace NewsletterSaver.Tests {
    [TestFixture]
    internal sealed class InternalConfigManagerTest {
        [Test]
        public void Read_IfFileNotExists_ReturnsNull() {
            // arrange

            var Application = GetApllicationMock();
            var File = GetFileWrapMock(false);
            InternalConfigManager Manager = new InternalConfigManager(File.Object, Application.Object);
            // action
            var Date = Manager.GetMaxDate();
            // assert
            Assert.IsNull(Date);
        }


        [Test]
        public void Read_IfFileExists_ReturnsDate() {
            // arrange
            var File = GetFileWrapMock(true);
            var Application = GetApllicationMock();

            File.Setup(s => s.ReadAllLines(@"C:\archivos de programa\test\maxdate.txt")).Returns(new[] {@"07/08/2013"});
            InternalConfigManager Manager = new InternalConfigManager(File.Object, Application.Object);
            // action
            var Date = Manager.GetMaxDate();
            // assert
            Assert.AreEqual(DateTime.Parse("07/08/2013 00:00"), Date.Value);
        }

        private static Mock<IApplicationWrap> GetApllicationMock() {
            var Application = new Mock<IApplicationWrap>();
            Application.Setup(d => d.ExecutablePath).Returns(@"C:\archivos de programa\test\");
            return Application;
        }

        private static Mock<IFileWrap> GetFileWrapMock(bool exist) {
            var File = new Mock<IFileWrap>();
            File.Setup(s => s.Exists(@"C:\archivos de programa\test\maxdate.txt")).Returns(exist);
            return File;
        }
    }
}