using System;

namespace NewsletterSaver {
    public struct Message : IMessage {
        private readonly string _Title;
        private readonly string _Text;
        private readonly DateTime? _Date;
        private readonly string _Sender;
        private readonly bool _IsHtml;
        private readonly string _From;

        public Message(string @from, string title, string text, DateTime date, string sender, bool isHtml) {
            _Title = title;
            _Text = text;
            _Date = date;
            _Sender = sender;
            _IsHtml = isHtml;
            _From = from;
        }
        
        
        public string From {
            get {
                return _From;
            }
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

        public DateTime? Date {
            get {
                return _Date;
            }
        }

        public string Sender {
            get {
                return _Sender;
            }
        }

        public bool IsHtml {
            get {
                return _IsHtml;
            }
        }
    }
}