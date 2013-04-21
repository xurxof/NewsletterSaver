using System.Collections.Generic;

namespace NewsletterSaver {
    public sealed class HtmlRemoteToLocalConverter {
        public IInMemoryDoc Convert(string memory) {
            if (memory == null)
                return new InMemoryDoc();
            return new InMemoryDoc(memory);
        }
    }

    public sealed class InMemoryDoc : IInMemoryDoc {
        private readonly IEnumerable<BinaryReferences> _BinaryReferences;


        public InMemoryDoc(string text) :this(){
            Text = text;
        }

        public InMemoryDoc() {
            Text = null;
            _BinaryReferences=new List<BinaryReferences> ();
        }
        public string Text { get; private set; }
        public IEnumerable<BinaryReferences> BinaryReferences {
            get {
                return _BinaryReferences;
            }
            
        }
    }

    public interface IInMemoryDoc {
        string Text { get; }
        IEnumerable<BinaryReferences> BinaryReferences { get; }
    }
}