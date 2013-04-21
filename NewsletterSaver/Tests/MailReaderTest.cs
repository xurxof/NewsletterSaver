﻿using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NewsletterSaver.Tests {
    [TestFixture]
    internal sealed class MailReaderTest {
        [Test]
        public void GetUnreadMails_IfNotUnreadMailExists_ReturnsEmptyEnumeration() {

            Pop3ClientFake PopClient = new Pop3ClientFake();
            MailReader Reader = new MailReader(PopClient, new MailFilterFake());
            IEnumerable<IMessage> Emails = Reader.GetUnreadMails();
            Assert.AreEqual(0, Emails.Count());

        }


        [Test]
        public void GetUnreadMail_ThereAreOneMail_ReturnsTheMail() {
            string ValidEmail = "pepejuan@gmail.com";
            var MessageFake = new MessageFake(ValidEmail);
            Pop3ClientFake PopClient = new Pop3ClientFake(MessageFake);
            MailReader Reader = new MailReader(PopClient, new MailFilterFake(ValidEmail));
            IEnumerable<IMessage> Emails = Reader.GetUnreadMails().ToList();
            Assert.AreEqual(1, Emails.Count());
            Assert.AreSame(MessageFake, Emails.First());
        }

        [Test]
        public void GetUnreadMail_ThereAreTargetAndNotTargetMails_ReturnsTargetFilter() {
            // arrange
            string ValidFrom = "pepejuan@gmail.com";
            var ValidMessageFake = new MessageFake(ValidFrom);
            Pop3ClientFake PopClient = new Pop3ClientFake(ValidMessageFake, new MessageFake("other@gmail.com"));
            MailReader Reader = new MailReader(PopClient, new MailFilterFake(ValidFrom));
            // action
            IEnumerable<IMessage> Emails = Reader.GetUnreadMails().ToList();
            // assert
            Assert.AreEqual(1, Emails.Count());
            Assert.AreSame(ValidMessageFake, Emails.First());
        }


        private sealed class Pop3ClientFake:IPop3Client {
            private readonly List<IMessage> _InnerList = new List<IMessage>();
            public Pop3ClientFake(params IMessage[] mesagge) {
                foreach (var Message in mesagge) {
                    _InnerList.Add(Message);
                }
                
            }
            public int GetMessageCount() {
                return _InnerList.Count();
            }

            public IMessage GetMessage(int messageNumber) {
                return _InnerList[messageNumber - 1];
            }
        }
        private sealed class MessageFake : IMessage {
            private readonly string _From;

            public MessageFake  (string fromHeader) {
                _From =fromHeader;
            }

            public IMessageHeader GetMessageHeaders(int mesaggerNumber) {
                return new MessageHeaderFake(_From);
            }
        }
        
    }

    internal sealed class MailFilterFake : IMailFilter {
        private readonly string _Valid;
        public MailFilterFake() {}

        public MailFilterFake(string validFrom) {
            _Valid=validFrom;
        }

        public bool IsHeaderAccepted(IMessageHeader iMessageHeader) {
            return iMessageHeader.From == _Valid;
        }
    }

    internal sealed class MessageHeaderFake : IMessageHeader {
        private readonly string _From;

        public MessageHeaderFake(string fromHeader) {
            _From = fromHeader;
        }

        public string From {
            get {
                return _From;
            }
        }

    }
}