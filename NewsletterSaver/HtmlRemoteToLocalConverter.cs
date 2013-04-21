using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;

namespace NewsletterSaver {
    public sealed class HtmlRemoteToLocalConverter {
        private readonly IWebFacade _WebFacade;

        public HtmlRemoteToLocalConverter(IWebFacade webRequest) {
            _WebFacade = webRequest;
        }

        public IInMemoryDoc Convert(string htmlString) {
            if (htmlString == null) {
                return new InMemoryDoc();
            }
            InMemoryDoc ReturnValue = new InMemoryDoc(htmlString);
            HtmlDocument Doc = new HtmlDocument();
            Doc.LoadHtml(htmlString);
            foreach (HtmlNode Node in GetNodes(Doc)) {
                ValuesFromNode Values = ExtractValuesFromNode(Node);
                
                Node.SetAttributeValue("src", Values.FileName);
                if (ReturnValue.ContainsBinaryReference(Values.AtributeValue))
                    continue;
                var BinaryReference = new BinaryReference(Values.AtributeValue, Values.FileName, Values.BinaryValue);
                ReturnValue.AddBinaryReference(BinaryReference);
            }
            return ReturnValue;
        }

        private IEnumerable<HtmlNode>  GetNodes(HtmlDocument doc) {
            return doc.DocumentNode.Descendants("img");
        }

        private ValuesFromNode ExtractValuesFromNode(HtmlNode node) {
            string Atrb = node.GetAttributeValue("src", null);
            string FileName = Path.GetFileName(Atrb);
            var BinaryValue = _WebFacade.GetBinaryRemoteFile(Atrb);
            
            return new ValuesFromNode(Atrb, BinaryValue, FileName);
        }
    }

    internal sealed class ValuesFromNode {
        public string AtributeValue { get; private set; }

        public byte[] BinaryValue { get; private set; }
        public string FileName { get; private set; }

        public ValuesFromNode(string atributeValue, byte[] binaryValue, string fileName) {
            AtributeValue = atributeValue;

            BinaryValue = binaryValue;
            FileName = fileName;
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

        internal bool ContainsBinaryReference(string originalLink) {
           return  _BinaryReferences.FirstOrDefault(b => b.OriginalLink == originalLink) != null;
        }
    }
}