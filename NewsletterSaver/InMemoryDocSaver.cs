using SystemWrapper.IO;

namespace NewsletterSaver {
    public sealed class InMemoryDocSaver {
        private readonly IFileWrap _FileWrap;

        public InMemoryDocSaver(IFileWrap fileWrap) {
            _FileWrap = fileWrap;
        }

        public bool Save(InMemoryDoc doc, string filePath) {
            if (doc == null) {
                return false;
            }
            _FileWrap.WriteAllText(filePath, doc.Text);
            foreach (BinaryReference reference in doc.BinaryReferences) {
                _FileWrap.WriteAllBytes(reference.NewLocalLink, reference.BinaryValue);
            }
            return true;
        }
    }
}