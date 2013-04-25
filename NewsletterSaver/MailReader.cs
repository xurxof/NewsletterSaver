using System;
using System.Linq;
using SystemWrapper;
using NLog;


namespace NewsletterSaver {
    internal sealed class MailReader {
        private readonly IMailClient _Client;
        private readonly IMailFilter _Filter;
        private readonly IDateTimeWrap _DateTime;
        private static readonly Logger _Logger = LogManager.GetCurrentClassLogger();
        public MailReader(IMailClient client, IMailFilter filter, IDateTimeWrap dateTime) {
            _Client = client;
            _Filter = filter;
            _DateTime = dateTime;
        }

        public ReadMailsResult GetUnreadMails(DateTime? datetime) {
                    
            DateTime DateTimeFilter;
            if (!datetime.HasValue) {
                _Logger.Debug("No datetime was specified reading messages. ");
                DateTimeFilter = _DateTime.Now.DateTimeInstance.AddHours(-24);
            }
            else {
                DateTimeFilter = datetime.Value;
            }
            _Logger.Debug("Reading mails arrived after {0}", DateTimeFilter);
            var mails = _Client.GetMessagesAfter(DateTimeFilter);
            _Logger.Debug("{0} mails readed", mails.Count());
            var Enumerable = mails as IMessage[] ?? mails.ToArray();
            DateTime MaxDate = (Enumerable.Any(p=>p.Date!=null) ? (DateTime) Enumerable.Where(p=>p.Date!=null).Max(m => m.Date) : DateTime.MinValue);
            var filteredMails=Enumerable.Where(m => _Filter.IsMessageAccepted(m));
            _Logger.Debug("{0} mails filtered", mails.Count());
            return new ReadMailsResult(filteredMails, MaxDate);
        }
    }
}