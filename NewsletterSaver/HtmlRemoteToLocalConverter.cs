using System.Collections.Generic;
using System.IO;
using HtmlAgilityPack;

namespace NewsletterSaver {
    public sealed class HtmlRemoteToLocalConverter {
        private readonly IWebFacade _WebFacade;

        public HtmlRemoteToLocalConverter(IWebFacade webRequest) {
            _WebFacade = webRequest;
        }

        public IInMemoryDoc Convert(string htmlString, string futureFileName) {
            if (htmlString == null) {
                return new InMemoryDoc();
            }
            string RelativeFilder = "\\" + Path.GetFileNameWithoutExtension(futureFileName) + " files\\";
            InMemoryDoc ReturnValue = new InMemoryDoc(htmlString);
            HtmlDocument Doc = new HtmlDocument();
            Doc.LoadHtml(htmlString);
            var Nodes = Doc.DocumentNode.Descendants("img");

            foreach (HtmlNode Node in Nodes) {
                string Atrb = Node.GetAttributeValue("src", null);
                string FileName = Path.GetFileName(Atrb);
                var BinaryValue = _WebFacade.GetBinaryRemoteFile(Atrb);

                string NewLocalLink = RelativeFilder + FileName;
                Node.SetAttributeValue("src", NewLocalLink);
                ReturnValue.AddBinaryReference(new BinaryReference(Atrb, NewLocalLink, BinaryValue));
            }
            return ReturnValue;
        }
    }

    public sealed class InMemoryDoc : IInMemoryDoc {
        private readonly List<BinaryReference> _BinaryReferences;


        public InMemoryDoc(string text) : this() {
            Text = text;
        }

        public InMemoryDoc() {
            Text = null;
            _BinaryReferences = new List<BinaryReference>();
        }

        public string Text { get; private set; }

        public IEnumerable<BinaryReference> BinaryReferences {
            get {
                return _BinaryReferences;
            }
        }

        internal void AddBinaryReference(BinaryReference binaryReference) {
            _BinaryReferences.Add(binaryReference);
        }
    }

    public interface IInMemoryDoc {
        string Text { get; }
        IEnumerable<BinaryReference> BinaryReferences { get; }
    }
}