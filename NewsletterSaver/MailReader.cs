using System.Collections.Generic;

namespace NewsletterSaver.Tests {
    internal class MailReader {
        public IEnumerable<IMail> GetUnreadMails() {
            return new List<IMail> ();
        }
    }
}