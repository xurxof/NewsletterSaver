using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;

namespace NewsletterSaver.Tests {
    [TestFixture]
    internal sealed class MailReaderTest {
        [Test]
        public void GetUnreadMails_IfNotUnreadMailExists_ReturnsEmptyEnumeration() {
            MailReader Reader = new MailReader();
            Mock<IPop3Client> PopClient = new Mock<IPop3Client>();
            PopClient.Setup(p => p.GetMessageCount()).Returns(0);
            IEnumerable<IMail>  Emails = Reader.GetUnreadMails();
            Assert.AreEqual(0, Emails.Count());

        }
    }
}