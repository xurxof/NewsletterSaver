using System;
using System.Linq;
using SystemWrapper;

namespace NewsletterSaver {
    internal sealed class MailReader {
        private readonly IMailClient _Client;
        private readonly IMailFilter _Filter;
        private readonly IDateTimeWrap _DateTime;

        public MailReader(IMailClient client, IMailFilter filter, IDateTimeWrap dateTime) {
            _Client = client;
            _Filter = filter;
            _DateTime = dateTime;
        }

        public ReadMailsResult GetUnreadMails(DateTime? datetime) {
            DateTime DateTimeFilter;
            if (!datetime.HasValue) {
                DateTimeFilter = _DateTime.Now.DateTimeInstance.AddHours(-24);
            }
            else {
                DateTimeFilter = datetime.Value;
            }
            var mails = _Client.GetMessagesAfter(DateTimeFilter);
            DateTime MaxDate;
            var Enumerable = mails as IMessage[] ?? mails.ToArray();
            MaxDate = (Enumerable.Any() ? (DateTime) Enumerable.Max(m => m.Date) : DateTime.MinValue);

            return new ReadMailsResult(Enumerable.Where(m => _Filter.IsMessageAccepted(m)), MaxDate);
        }
    }
}