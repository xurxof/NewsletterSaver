namespace NewsletterSaver {
    public sealed class BinaryReference {
        private readonly string _OriginalLink;
        private readonly string _NewLocalLink;
        private readonly byte[] _BinaryValue;

        public BinaryReference(string originalLink, string newLocalLink, byte[] binaryValue) {
            _OriginalLink = originalLink;
            _NewLocalLink = newLocalLink;
            _BinaryValue = binaryValue;
        }

        public string OriginalLink {
            get {
                return _OriginalLink;
            }
        }

        public string NewLocalLink {
            get {
                return _NewLocalLink;
            }
        }

        public byte[] BinaryValue {
            get {
                return _BinaryValue;
            }
        }
    }
}