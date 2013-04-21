using SystemWrapper.IO;
using Moq;

namespace NewsletterSaver {
    internal sealed class MessageSaver {
        public MessageSaver(Mock<IFileWrap> file) {
        
        }

        public string Save(IMessage message) {
            return null;
        }
    }
}