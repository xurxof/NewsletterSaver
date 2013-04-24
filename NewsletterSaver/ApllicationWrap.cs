namespace NewsletterSaver {
    internal sealed class ApllicationWrap : IApplicationWrap {
        

        public string ExecutablePath {
            get {
                return System.Windows.Forms.Application.ExecutablePath;
            }
        }
    }
}