using System;
using System.Linq;
using System.Collections.Generic;
using AE.Net.Mail;

namespace NewsletterSaver {
    internal sealed class MailClient : IMailClient {
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


        public IEnumerable<IMessage> GetMessagesAfter(DateTime minDateTime) {
            var MessageList = new List<IMessage>();
            using (ImapClient Imap = new ImapClient(_Host, _Username, _Password, ImapClient.AuthMethods.Login, _Port, _IsSSL)) {
                var Messages = Imap.SearchMessages(SearchCondition.Undeleted().And(SearchCondition.SentSince(minDateTime))).ToArray ();
                foreach (var ImapMessage in Messages) {
                    //Message.Value.Load(Message.Value.Uid);

                    string from = ImapMessage.Value.From == null ? "" : ImapMessage.Value.From.Address;//  Message.Value.Headers.ContainsKey("From") ? Message.Value.Headers["From"].Value : "";
                    string sender = ImapMessage.Value.Sender== null? "":ImapMessage.Value.Sender.Address; 
                    string subject = ImapMessage.Value.Subject;// Headers.ContainsKey("Subject") ? Message.Value.Headers["Subject"].Value : "";
                    DateTime date = ImapMessage.Value.Date; //  Headers.ContainsKey("Date") ? DateTime.Parse(Message.Value.Headers["Date"].Value) : DateTime.MinValue;
                    
                    string body;
                    var htmlAlternateView = ImapMessage.Value.AlternateViews.FirstOrDefault(a => a.ContentType == "text/html");
                    if (htmlAlternateView !=null) {
                        
                        body = htmlAlternateView.Body;
                    }
                    else {
                        
                        body = ImapMessage.Value.Body ;
                    }

                    IMessage Message = new Message(ImapMessage.Value.From == null ? "" : from, subject, body, date, sender, htmlAlternateView!=null);

                    MessageList.Add(Message);
                }
            }
            return MessageList.ToArray();
        }
    }
}