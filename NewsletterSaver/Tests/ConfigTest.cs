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
            Lines.AddRange(new[] {"imapUser:pepejuan@gmail.com", "imapPass:Contraseña", "imapHost:imap.gmail.com", "imapPort:993", "imapUseSSL:true", @"savePath:c:\mails\", "fromFilter:uno@blabla.com", "fromFilter:dos@blabla.com", "fromFilter:tres@blabla.com"});
            Construct_WhenFileSpecified_ReadContent("pepejuan@gmail.com", "Contraseña", "imap.gmail.com", 993, true, @"c:\mails\", "uno@blabla.com", "dos@blabla.com", "tres@blabla.com", Lines);
            // reorder the file lines
            Lines = new List<string>();
            Lines.AddRange(new[] {@"savePath:c:\mails\", "fromFilter:dos@blabla.com", "fromFilter:tres@blabla.com", "imapUser:pepejuan@gmail.com", "imapPass:Contraseña", "imapHost:imap.gmail.com", "imapPort:993", "imapUseSSL:true", "fromFilter:uno@blabla.com"});
            Construct_WhenFileSpecified_ReadContent("pepejuan@gmail.com", "Contraseña", "imap.gmail.com", 993, true, @"c:\mails\", "uno@blabla.com", "dos@blabla.com", "tres@blabla.com", Lines);
        }

        [Test]
        public void Construct_IfFileNotExists_CreateNewDefaultValues() {
            var DefaultConfigStrings = new[] {"imapUser:pepejuan@gmail.com", "imapPass:password", "imapHost:imap.gmail.com", "imapPort:993", "imapUseSSL:true", @"savePath:c:\mails\", "fromFilter:uno@blabla.com", "fromFilter:dos@blabla.com", "fromFilter:tres@blabla.com"};
            var FileMock = new Mock<IFileWrap>();
            FileMock.Setup(f => f.Exists(@"c:\newslettersaver.cfg")).Returns(false);
            FileMock.Setup(f => f.WriteAllLines(@"c:\newslettersaver.cfg", DefaultConfigStrings));
            FileMock.Setup(f => f.ReadAllLines(@"c:\newslettersaver.cfg")).Returns(DefaultConfigStrings);    
            Config Cfg = new Config(@"c:\newslettersaver.cfg", FileMock.Object);

            Assert.AreEqual("pepejuan@gmail.com", Cfg.ImapUser);
            Assert.AreEqual("password", Cfg.ImapPass);
            Assert.AreEqual("imap.gmail.com", Cfg.ImapHost);
            Assert.AreEqual(993, Cfg.ImapPort);
            Assert.AreEqual(true, Cfg.ImapUseSSL);
            Assert.AreEqual(@"c:\mails\", Cfg.SavePath);
            Assert.AreEqual(3, Cfg.FromFilter.Count());
            Assert.AreEqual(1, Cfg.FromFilter.Count(f => f == "uno@blabla.com"));
            Assert.AreEqual(1, Cfg.FromFilter.Count(f => f == "dos@blabla.com"));
            Assert.AreEqual(1, Cfg.FromFilter.Count(f => f == "tres@blabla.com"));
        }

        public void Construct_WhenFileSpecified_ReadContent(string mail, string password, string host, int port, bool useSSL, string savePath, string fromFilter1, string fromFilter2, string fromFilter3, List<string> lines) {
            var FileMock = new Mock<IFileWrap>();

            FileMock.Setup(f => f.ReadAllLines(@"c:\newslettersaver.cfg")).Returns(lines.ToArray());
            FileMock.Setup(f => f.WriteAllLines(@"c:\newslettersaver.cfg", new[] {"imapUser:" + mail, "imapPass:" + password, "imapHost:" + host, "imapPort:" + port, "imapUseSSL:" + useSSL, @"savePath:" + savePath, "fromFilter:" + fromFilter1, "fromFilter:" + fromFilter2, "fromFilter:" + fromFilter3}));
            Config Cfg = new Config(@"c:\newslettersaver.cfg", FileMock.Object);

            Assert.AreEqual(mail, Cfg.ImapUser);
            Assert.AreEqual(password, Cfg.ImapPass);
            Assert.AreEqual(host, Cfg.ImapHost);
            Assert.AreEqual(port, Cfg.ImapPort);
            Assert.AreEqual(useSSL, Cfg.ImapUseSSL);
            Assert.AreEqual(savePath, Cfg.SavePath);
            Assert.AreEqual(3, Cfg.FromFilter.Count());
            Assert.AreEqual(1, Cfg.FromFilter.Count(f => f == fromFilter1));
            Assert.AreEqual(1, Cfg.FromFilter.Count(f => f == fromFilter2));
            Assert.AreEqual(1, Cfg.FromFilter.Count(f => f == fromFilter3));
        }
    }
}