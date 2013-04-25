using System;
using System.Collections.Generic;

namespace NewsletterSaver {
    public struct ReadMailsResult {
        public ReadMailsResult(IEnumerable<IMessage> mails, DateTime maxDate) : this() {
            MaxDate = maxDate;
            Mails = mails;
        }

        public DateTime MaxDate { get; private set; }

        public IEnumerable<IMessage> Mails { get; private set; }
    }
}