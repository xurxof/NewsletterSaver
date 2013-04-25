using System;
using System.Collections.Generic;
using System.Linq;
using SystemWrapper;
using Moq;
using NUnit.Framework;

namespace NewsletterSaver.Tests {
    [TestFixture]
    internal sealed class MailReaderTest {
        [Test]
        public void GetUnreadMails_IfNotUnreadMailExists_ReturnsEmptyEnumeration() {

            MailClientStub PopClient = new MailClientStub();
            var DateTime = GetDateTimeStrub("01/01/2013");
            MailReader Reader = new MailReader(PopClient, new MailFilterFake(), DateTime.Object);
            IEnumerable<IMessage> Emails = Reader.GetUnreadMails(null).Mails;
            Assert.AreEqual(0, Emails.Count());

        }

        private static Mock<IDateTimeWrap> GetDateTimeStrub(string dateTime) {
            Mock<IDateTimeWrap> DateTimeStrub = new Mock<IDateTimeWrap>();
            DateTimeStrub.Setup(d => d.Now.DateTimeInstance).Returns(DateTime.Parse(dateTime));
            return DateTimeStrub;
        }


        [Test]
        public void GetUnreadMail_ThereAreOneMail_ReturnsTheMail() {
            string ValidEmail = "pepejuan@gmail.com";
            var MessageFake = new MessageFake(ValidEmail);
            MailClientStub PopClient = new MailClientStub(MessageFake);
            var DateTime = GetDateTimeStrub("01/01/2013");
            MailReader Reader = new MailReader(PopClient, new MailFilterFake(ValidEmail), DateTime.Object);
            IEnumerable<IMessage> Emails = Reader.GetUnreadMails(null).Mails.ToList();
            Assert.AreEqual(1, Emails.Count());
            Assert.AreSame(MessageFake, Emails.First());
        }

        [Test]
        public void GetUnreadMail_ThereAreTargetAndNotTargetMails_ReturnsTargetFilter() {
            // arrange
            string ValidFrom = "pepejuan@gmail.com";
            var ValidMessageFake = new MessageFake(ValidFrom);
            MailClientStub PopClient = new MailClientStub(ValidMessageFake, new MessageFake("other@gmail.com"));

            var DateTime = GetDateTimeStrub("01/01/2013");
            MailReader Reader = new MailReader(PopClient, new MailFilterFake(ValidFrom), DateTime.Object);
            // action
            IEnumerable<IMessage> Emails = Reader.GetUnreadMails(null).Mails.ToList();
            // assert
            Assert.AreEqual(1, Emails.Count());
            Assert.AreSame(ValidMessageFake, Emails.First());
        }


        private sealed class MailClientStub:IMailClient {
            private readonly List<IMessage> _InnerList = new List<IMessage>();
            public MailClientStub(params IMessage[] mesagge) {
                foreach (var Message in mesagge) {
                    _InnerList.Add(Message);
                }
                
            }
            
            public IEnumerable<IMessage> GetMessagesAfter(DateTime minDateTime) {
                return _InnerList.AsEnumerable();
            }
        }
        private sealed class MessageFake : IMessage {
            private readonly string _From;

            
            public MessageFake  (string fromHeader) {
                _From =fromHeader;
            }

        

            public bool IsHtml {
                get {
                    return false;
                }
            }


            public string From {
                get {
                    return _From;
                }
            }

            public string Title {
                get {
                    return "title";
                }
            }

            public string Text {
                get {
                    return "";
                }
            }

            public DateTime? Date {
                get {
                    return null;
                }
            }

            public string Sender {
                get {
                    return "";
                }
            }
        }
        
    }

    internal sealed class MailFilterFake : IMailFilter {
        private readonly string _Valid;
        public MailFilterFake() {}

        public MailFilterFake(string validFrom) {
            _Valid=validFrom;
        }

        public bool IsMessageAccepted(IMessage message) {
            return message.From == _Valid;
        }
    }

    
}