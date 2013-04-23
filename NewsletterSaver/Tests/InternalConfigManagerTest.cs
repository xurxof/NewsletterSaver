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
            Mock<IFileWrap> File = new Mock<IFileWrap>();
            Mock<IApplicationWrap> Application= new Mock<IApplicationWrap>();
            Application.Setup(d=>d.ExecutablePath).Returns(@"C:\archivos de programa\test\");
            File.Setup(s=>s.Exists(@"C:\archivos de programa\test\maxdate.txt")).Returns(false);
            InternalConfigManager Manager = new InternalConfigManager(File.Object, Application.Object);
            // action
            DateTime? Date = Manager.GetMaxDate();
            // assert
            Assert.IsNull(Date);
        }
        [Test]
        public void Read_IfFileExists_ReturnsDate() {
            // arrange
            Mock<IFileWrap> File = new Mock<IFileWrap>();
            Mock<IApplicationWrap> Application = new Mock<IApplicationWrap>();
            Application.Setup(d => d.ExecutablePath).Returns(@"C:\archivos de programa\test\");
            File.Setup(s => s.Exists(@"C:\archivos de programa\test\maxdate.txt")).Returns(true);
            File.Setup(s => s.ReadAllLines(@"C:\archivos de programa\test\maxdate.txt")).Returns(new[] {@"07/08/2013"});
            InternalConfigManager Manager = new InternalConfigManager(File.Object, Application.Object);
            // action
            DateTime? Date = Manager.GetMaxDate();
            // assert
            Assert.AreEqual(DateTime.Parse("07/08/2013 00:00"),Date.Value);
        }
    }
}