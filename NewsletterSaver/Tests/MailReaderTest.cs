using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;

namespace NewsletterSaver.Tests {
    [TestFixture]
    internal class MailReaderTest {
        [Test]
        public void GetUnreadMails_IfNotUnreadMailExists_ReturnsEmptyEnumeration() {
            MailReader reader = new MailReader();
            Mock<IPop3Client> popClient = new Mock<IPop3Client>();
            popClient.Setup(p => p.GetMessageCount()).Returns(0);
            IEnumerable<IMail>  Emails = reader.GetUnreadMails();
            Assert.AreEqual(0, Emails.Count());

        }
    }
}