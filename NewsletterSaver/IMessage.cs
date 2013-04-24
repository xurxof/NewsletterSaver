using System;

namespace NewsletterSaver {
    public interface IMessage {
        string From { get; }
        string Title { get; }
        string Text { get; }
        DateTime? Date { get; }
        string Sender { get; }
        bool IsHtml { get; }
    }
}