namespace NewsletterSaver.Tests {
    public interface IPop3Client {
        int GetMessageCount();

        IMessage GetMessage(int messageNumber);
    }
}