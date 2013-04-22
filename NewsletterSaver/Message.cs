namespace NewsletterSaver {
    public sealed class Message : IMessage {
        private readonly MessageHeader _Header;

        private readonly string _Title;

        private readonly string _Text;


        private readonly ContentsTypes _Content;

        public Message(string from, string title, string text, ContentsTypes content) {
            _Title = title;
            _Text = text;
            _Content = content;
            _Header = new MessageHeader(from);
        }


        IMessageHeader IMessage.GetMessageHeader() {
            return _Header;
        }

        public string Title {
            get {
                return _Title;
            }
        }

        public string Text {
            get {
                return _Text;
            }
        }


        public ContentsTypes Content {
            get {
                return _Content;
            }
        }
    }
}