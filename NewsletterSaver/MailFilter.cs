using System.Collections.Generic;
using System.Linq;
using NLog;


namespace NewsletterSaver {
    public sealed class MailFilter : IMailFilter {
        private static readonly Logger _Logger = LogManager.GetCurrentClassLogger();
        private readonly IEnumerable<string> _Froms;

        public MailFilter(params string[] froms) {
            _Froms = froms.Where(f => !string.IsNullOrWhiteSpace(f)).Select (f=>f.TrimEnd(' ').TrimStart(' '));
        }

        public bool IsMessageAccepted(IMessage message) {
            var Accepted=_Froms.Any(f => f == message.From || f == message.Sender);

            _Logger.Debug("Mesagge with mails {0} and {1} {2} acepted.", message.From, message.Sender, (Accepted ? "was" : "was not"));
            return Accepted;
        }
    }
}