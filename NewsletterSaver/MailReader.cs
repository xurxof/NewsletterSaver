using System.Collections.Generic;
using NewsletterSaver.Tests;

namespace NewsletterSaver {
    internal sealed class MailReader {
        private readonly IPop3Client _Client;
        private readonly IMailFilter _Filter;

        public MailReader(IPop3Client client, IMailFilter filter) {
            _Client = client;
            _Filter = filter;
        }

        public IEnumerable<IMessage> GetUnreadMails() {
            int MessageCount = _Client.GetMessageCount();
            if (MessageCount == 0) {
                return new List<IMessage>();
            }
            return FillMessageList(MessageCount);
        }

        private IEnumerable<IMessage> FillMessageList(int messageCount) {
            var Mails = new List<IMessage>();
            for (int I = messageCount; I > 0; I--) {
                IMessage Message = _Client.GetMessage(I);
                if (_Filter.IsHeaderAccepted(Message.GetMessageHeaders(I))) {
                    Mails.Add(Message);
                }
            }
            return Mails;
        }
    }
}