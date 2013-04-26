using System.Collections.Generic;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using NLog;

namespace NewsletterSaver {
    public sealed class HtmlRemoteToLocalConverter {
        private readonly IWebFacade _WebFacade;

        private static readonly Logger _Logger = LogManager.GetCurrentClassLogger();

        public HtmlRemoteToLocalConverter(IWebFacade webRequest) {
            _WebFacade = webRequest;
        }

        public IInMemoryDoc Convert(string htmlString, string newSubDirectoryName, string outputDirectory, string newTitle) {
            if (htmlString == null) {
                return new InMemoryDoc();
            }


            InMemoryDoc ReturnValue = new InMemoryDoc(htmlString);
            HtmlDocument Doc = new HtmlDocument();
            Doc.LoadHtml(htmlString);
            var TitleNode = Doc.DocumentNode.Descendants("title").FirstOrDefault() ;
            if (TitleNode != null) {
                Doc.DocumentNode.InnerHtml = Doc.DocumentNode.InnerHtml.Replace(TitleNode.InnerHtml, "<title>"+newTitle+"</titlel>");
            }
            foreach (HtmlNode Node in GetImageNodes(Doc)) {
                _Logger.Debug("Image node with value {0} was founded.", Node.GetAttributeValue("src", string.Empty));
                ValuesFromNode Values = ExtractValuesFromNode(Node, newSubDirectoryName);
                _Logger.Debug("New value of node: {0} .", Values.FileName);
                Node.SetAttributeValue("src", Values.FileName);
                if (ReturnValue.ContainsBinaryReference(Values.AtributeValue)) {
                    continue;
                }
                BinaryReference BinaryReference = new BinaryReference(Values.AtributeValue, Path.Combine(outputDirectory, Values.FileName), Values.BinaryValue);
                ReturnValue.AddBinaryReference(BinaryReference);
            }
            ReturnValue.Text = Doc.DocumentNode.InnerHtml;
            return ReturnValue;
        }

        private IEnumerable<HtmlNode> GetImageNodes(HtmlDocument doc) {
            return doc.DocumentNode.Descendants("img");
        }

        private ValuesFromNode ExtractValuesFromNode(HtmlNode node, string newDirectoryName) {
            string Atrb = node.GetAttributeValue("src", null);
            string FileName = Path.Combine(newDirectoryName, GetLocalFileNameForRemoteImage(Atrb));
            byte[] BinaryValue;
            try {
                BinaryValue = _WebFacade.GetBinaryRemoteFile(Atrb);
            }
            catch {
                // all the exceptions are ignored; the references are not prioritary
                BinaryValue = new byte[] {};
            }

            return new ValuesFromNode(Atrb, BinaryValue, FileName);
        }

        private static string GetLocalFileNameForRemoteImage(string atrb) {
            string Extension = Path.GetExtension(atrb);
            if (string.IsNullOrWhiteSpace(Extension)) {
                Extension = ".jpg";
            }
            if (Extension.Length > 4) {
                Extension = Extension.Substring(0, 4);
            }
            string LocalFileNameForRemoteImage = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Extension;
            return LocalFileNameForRemoteImage;
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

        public string Text { get; set; }

        public IEnumerable<BinaryReference> BinaryReferences {
            get {
                return _BinaryReferences;
            }
        }

        internal void AddBinaryReference(BinaryReference binaryReference) {
            _BinaryReferences.Add(binaryReference);
        }

        internal bool ContainsBinaryReference(string originalLink) {
            return _BinaryReferences.FirstOrDefault(b => b.OriginalLink == originalLink) != null;
        }
    }
}