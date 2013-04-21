namespace NewsletterSaver {
    public interface IWebFacade {
        byte[] GetBinaryRemoteFile(string url);
    }
}