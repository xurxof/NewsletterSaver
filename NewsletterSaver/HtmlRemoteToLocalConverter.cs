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
            // TODO: create path and file calculator for this kind of lines
            string RelativeFolder = "\\" + Path.GetFileNameWithoutExtension(futureFileName) + " files\\";
            InMemoryDoc ReturnValue = new InMemoryDoc(htmlString);
            HtmlDocument Doc = new HtmlDocument();
            Doc.LoadHtml(htmlString);
            foreach (HtmlNode Node in GetNodes(Doc)) {
                ValuesFromNode Values = ExtractValuesFromNode(Node, RelativeFolder);
                Node.SetAttributeValue("src", Values.NewLocalLink);
                var BinaryReference = new BinaryReference(Values.AtributeValue, Values.NewLocalLink, Values.BinaryValue);
                ReturnValue.AddBinaryReference(BinaryReference);
            }
            return ReturnValue;
        }

        private IEnumerable<HtmlNode>  GetNodes(HtmlDocument doc) {
            return doc.DocumentNode.Descendants("img");
        }

        private ValuesFromNode ExtractValuesFromNode(HtmlNode node, string relativeFilder) {
            string Atrb = node.GetAttributeValue("src", null);
            string FileName = Path.GetFileName(Atrb);
            var BinaryValue = _WebFacade.GetBinaryRemoteFile(Atrb);

            string NewLocalLink = relativeFilder + FileName;
            return new ValuesFromNode(Atrb, BinaryValue, NewLocalLink);
        }
    }

    internal sealed class ValuesFromNode {
        public string AtributeValue { get; private set; }

        public byte[] BinaryValue { get; private set; }
        public string NewLocalLink { get; private set; }

        public ValuesFromNode(string atributeValue, byte[] binaryValue, string newLocalLink) {
            AtributeValue = atributeValue;

            BinaryValue = binaryValue;
            NewLocalLink = newLocalLink;
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