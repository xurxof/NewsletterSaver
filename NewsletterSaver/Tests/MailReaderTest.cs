using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;

namespace NewsletterSaver.Tests {
    [TestFixture]
    internal sealed class MailReaderTest {
        [Test]
        public void GetUnreadMails_IfNotUnreadMailExists_ReturnsEmptyEnumeration() {
            
            Mock<IPop3Client> PopClient = new Mock<IPop3Client>();
            PopClient.Setup(p => p.GetMessageCount()).Returns(0);
            MailReader Reader = new MailReader(PopClient.Object);
            IEnumerable<IMessage> Emails = Reader.GetUnreadMails();
            Assert.AreEqual(0, Emails.Count());

        }


        [Test]
        public void GetUnreadMail_ThereAreOneMail_ReturnsTheMail() {
            
            Mock<IPop3Client> PopClient = new Mock<IPop3Client>();
            PopClient.Setup(p => p.GetMessageCount()).Returns(1);
            Mock<IMessage> Message = new Mock<IMessage>();
            PopClient.Setup(p => p.GetMessage(1)).Returns(Message.Object);
            MailReader Reader = new MailReader(PopClient.Object);
            IEnumerable<IMessage> Emails = Reader.GetUnreadMails();
            Assert.AreEqual(1, Emails.Count());
            Assert.AreSame(Message.Object, Emails.First());
        }
	  
    }
}