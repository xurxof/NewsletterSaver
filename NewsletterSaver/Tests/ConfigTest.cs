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
            Construct_WhenFileSpecified_ReadContent("pepejuan@gmail.com", "Contraseña", "pop.gmail.com", 995, true, @"c:\mails\", "uno@blabla.com", "dos@blabla.com", "tres@blabla.com", Lines);
            // reorder the file lines
            Lines = new List<string>();
            Lines.AddRange(new[] {@"savePath:c:\mails\", "fromFilter:dos@blabla.com", "fromFilter:tres@blabla.com", "popUser:pepejuan@gmail.com", "popPass:Contraseña", "popHost:pop.gmail.com", "popPort:995", "popUseSSL:true", "fromFilter:uno@blabla.com"});
            Construct_WhenFileSpecified_ReadContent("pepejuan@gmail.com", "Contraseña", "pop.gmail.com", 995, true, @"c:\mails\", "uno@blabla.com", "dos@blabla.com", "tres@blabla.com", Lines);
        }

        public void Construct_WhenFileSpecified_ReadContent(string mail, string password, string host, int port, bool useSSL, string savePath, string fromFilter1, string fromFileter2, string fromFiler3, List<string> lines) {
            var FileMock = new Mock<IFileWrap>();

            FileMock.Setup(f => f.ReadAllLines(@"c:\newslettersaver.cfg")).Returns(lines.ToArray());

            Config Cfg = new Config(@"c:\newslettersaver.cfg", FileMock.Object);

            Assert.AreEqual(mail, Cfg.PopUser);
            Assert.AreEqual(password, Cfg.PopPass);
            Assert.AreEqual(host, Cfg.PopHost);
            Assert.AreEqual(port, Cfg.PopPort);
            Assert.AreEqual(useSSL, Cfg.PopUseSSL);
            Assert.AreEqual(savePath, Cfg.SavePath);
            Assert.AreEqual(3, Cfg.FromFilter.Count());
            Assert.AreEqual(1, Cfg.FromFilter.Count(f => f == fromFilter1));
            Assert.AreEqual(1, Cfg.FromFilter.Count(f => f == fromFileter2));
            Assert.AreEqual(1, Cfg.FromFilter.Count(f => f == fromFiler3));
        }
    }
}