using System.Collections.Generic;

namespace NewsletterSaver {
    public interface IInMemoryDoc {
        string Text { get; }
        IEnumerable<BinaryReference> BinaryReferences { get; }
    }
}