using System.Collections.Generic;
using System.Linq;

namespace NewsletterSaver {
    public sealed class MailFilter : IMailFilter {
        private readonly IEnumerable<string> _Froms;

        public MailFilter(params string[] froms) {
            _Froms = froms.Where(f => !string.IsNullOrWhiteSpace(f)).Select (f=>f.TrimEnd(' ').TrimStart(' '));
        }

        public bool IsMessageAccepted(IMessage message) {
            return _Froms.Any(f => f == message.From || f == message.Sender);
        }
    }
}