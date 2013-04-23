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
	  
    }
}