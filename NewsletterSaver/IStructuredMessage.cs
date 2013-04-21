namespace NewsletterSaver {
    public interface IStructuredMessage {
        string Title { get; }
        string Text { get; }
        ContentsTypes Content { get; }
    }
}