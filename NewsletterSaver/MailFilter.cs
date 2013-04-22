
using System.Collections.Specialized;

namespace NewsletterSaver {
    public sealed class MailFilter : IMailFilter {
        private StringCollection _Froms;
        public MailFilter(params string [] froms) {
            _Froms = new StringCollection();
            _Froms.AddRange(froms);
        }

        public bool IsHeaderAccepted(IMessageHeader header) {
            return _Froms.Contains(header.From);
        }
    }
}
