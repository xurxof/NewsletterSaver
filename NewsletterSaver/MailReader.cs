using System.Collections.Generic;
using NewsletterSaver.Tests;

namespace NewsletterSaver {
    internal sealed class MailReader {
        public IEnumerable<IMail> GetUnreadMails() {
            return new List<IMail>();
        }
    }
}