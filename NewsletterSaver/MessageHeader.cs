namespace NewsletterSaver {

    public sealed class MessageHeader : IMessageHeader {
    private readonly string _From;

    public MessageHeader(string from) {
        _From = @from;
    }


        public string From {
            get {
                return _From;
            }
        }
    }

}