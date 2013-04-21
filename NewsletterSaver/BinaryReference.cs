namespace NewsletterSaver {
    public sealed class BinaryReference {
        private readonly string _OriginalLink;
        private readonly string _LocalFilename;
        private readonly byte[] _BinaryValue;

        public BinaryReference(string originalLink, string localFileName, byte[] binaryValue) {
            _OriginalLink = originalLink;
            _LocalFilename = localFileName;
            _BinaryValue = binaryValue;
        }

        public string OriginalLink {
            get {
                return _OriginalLink;
            }
        }

        public string NewRealtiveLocalLink {
            get {
                return _LocalFilename;
            }
        }

        public byte[] BinaryValue {
            get {
                return _BinaryValue;
            }
        }
    }
}