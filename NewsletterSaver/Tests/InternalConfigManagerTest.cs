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
        public void Read_IfFileIsEmpty_ReturnsNull() {
            // arrange

            var Application = GetApllicationMock();
            var File = GetFileWrapMock(false);
            File.Setup(s => s.ReadAllLines(@"C:\archivos de programa\test\maxdate.txt")).Returns(new string[] {});
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
            Assert.AreEqual(DateTime.Parse("08/07/2013 00:00"), Date.Value);
        }



        [Test]
        public void Save_WhenEnds_SaveMaxDate() {
            // arrange
            var File = GetFileWrapMock(true);
            var Application = GetApllicationMock();

            File.Setup(s => s.WriteAllLines(@"C:\archivos de programa\test\maxdate.txt",new [] {@"08/07/2013 00:00:00"}));
            InternalConfigManager Manager = new InternalConfigManager(File.Object, Application.Object);
            DateTime DateTimeToSave = DateTime.Parse("08/07/2013");
            // action
            bool Saved = Manager.SaveDate(DateTimeToSave);
            // assert
            Assert.IsTrue(Saved);
            File.Verify (m=>m.WriteAllText(@"C:\archivos de programa\test\maxdate.txt",@"07/08/2013 00:00:00"), Times.Once ());
            
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