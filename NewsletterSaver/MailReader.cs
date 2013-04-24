using System;
using System.Linq;
using System.Collections.Generic;
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

        public IEnumerable<IMessage> GetUnreadMails(DateTime? datetime) {
            DateTime DateTimeFilter;
            if (!datetime.HasValue)
                DateTimeFilter = _DateTime.Now.DateTimeInstance.AddHours(-24);
            else
                DateTimeFilter = datetime.Value;
            return _Client.GetMessagesAfter(DateTimeFilter).Where(m=>_Filter.IsHeaderAccepted( m.GetMessageHeaders()));
        }
    }
}