using System.Collections.Generic;
using NewsletterSaver.Tests;

namespace NewsletterSaver {
    internal sealed class MailReader {
        private readonly IPop3Client _Client;

        public MailReader(IPop3Client client) {
            _Client = client;
        }

        public IEnumerable<IMessage> GetUnreadMails() {
            

            int MessageCount = _Client.GetMessageCount();
            if (MessageCount == 0) {
                return new List<IMessage>();
            }
            return FillMessageList( MessageCount);
            
        }

        private IEnumerable<IMessage> FillMessageList( int messageCount) {
            var Mails = new List<IMessage>();
            for (int I = messageCount; I > 0; I--) {
                Mails.Add(_Client.GetMessage(I));
            }
            return Mails;
        }
    }
}