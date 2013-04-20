using System.Collections.Generic;
using System.Linq;
using SystemWrapper.IO;
using Moq;
using NUnit.Framework;

namespace NewsletterSaver.Tests {
    [TestFixture]
    internal sealed class ConfigTest {
        [Test]
        public void Construct_WhenFileSpecified_ReadContent() {
            var Lines = new List<string>();
            Lines.AddRange(new[] {"popUser:pepejuan@gmail.com", "popPass:Contraseña", "popHost:pop.gmail.com", "popPort:995", "popUseSSL:true", @"savePath:c:\mails\", "fromFilter:uno@blabla.com", "fromFilter:dos@blabla.com", "fromFilter:tres@blabla.com"});
            var FileMock = new Mock<IFileWrap>();

            FileMock.Setup(f => f.ReadAllLines(@"c:\newslettersaver.cfg")).Returns(Lines.ToArray());

            Config Cfg = new Config(@"c:\newslettersaver.cfg", FileMock.Object);

            Assert.AreEqual("pepejuan@gmail.com", Cfg.PopUser);
            Assert.AreEqual("Contraseña", Cfg.PopPass);
            Assert.AreEqual("pop.gmail.com", Cfg.PopHost);
            Assert.AreEqual(995, Cfg.PopPort);
            Assert.AreEqual(true, Cfg.PopUseSSL);
            Assert.AreEqual(@"c:\mails\", Cfg.SavePath);
            Assert.AreEqual(3, Cfg.FromFilter.Count());
            Assert.AreEqual(1, Cfg.FromFilter.Count(f => f == "uno@blabla.com"));
            Assert.AreEqual(1, Cfg.FromFilter.Count(f => f == "dos@blabla.com"));
            Assert.AreEqual(1, Cfg.FromFilter.Count(f => f == "tres@blabla.com"));
        }
    }
}