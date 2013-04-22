using System.Collections.Generic;
using AE.Net.Mail;
using IMailClient = NewsletterSaver.Tests.IMailClient;

namespace NewsletterSaver {
    public class MailClient :IMailClient{
        private readonly bool _IsSSL;
        private readonly string _Host;
        private readonly string _Username;
        private readonly string _Password;
        private readonly int _Port;

        public MailClient(bool isSSL, string host, string username, string password, int port) {
            _IsSSL = isSSL;
            _Host = host;
            _Username = username;
            _Password = password;
            _Port = port;
        }



        public IMessage[] GetMessages() {
            List<IMessage> MessageList = new List<IMessage>();
            using (var Imap = new ImapClient(_Host, _Username, _Password, ImapClient.AuthMethods.Login, _Port, _IsSSL)) {
                // TODO: read from last check!
                // TODO: read only filtered mails!
                // sample: SearchCondition.Undeleted().And(SearchCondition.From("david"), SearchCondition.SentSince(new DateTime(2000, 1, 1))

                var Messages =Imap.SearchMessages(SearchCondition.Undeleted());
                foreach (var Message in Messages) {
                    Message.Value.Load(Message.Value.Uid);
                    IMessage Meessage = new Message(Message.Value.From.Address, Message.Value.Subject, , ContentsTypes.Text);
                    MessageList.Add(Meessage);
                }
                
            }
            return MessageList.ToArray();
        }

    }
}