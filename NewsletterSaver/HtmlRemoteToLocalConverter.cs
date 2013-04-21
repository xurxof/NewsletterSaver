namespace NewsletterSaver {
    public sealed class HtmlRemoteToLocalConverter {
        public IInMemoryDoc   Convert(string memory) {
            return new InMemoryDoc();
        }
    }

    public class InMemoryDoc : IInMemoryDoc {
        public string Text { get; private set; }
    }

    public interface IInMemoryDoc { string Text { get; } }
}
