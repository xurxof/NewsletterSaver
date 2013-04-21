using SystemWrapper.IO;

namespace NewsletterSaver {
    internal sealed class MessageSaver {
        private readonly IFileWrap _File;

        private readonly string _DestinyPath;
        private readonly IPathWrap _Path;


        public MessageSaver(IFileWrap file) {
            _File = file;
        }

        public MessageSaver(IFileWrap file, string destinyPath, IPathWrap path) {
            _File = file;
            _DestinyPath = destinyPath;
            _Path = path;
        }

        public string Save(IStructuredMessage message) {
            if (message == null) {
                return null;
            }
            string FileName = GetFilePath(_DestinyPath, message);
            _File.WriteAllText(FileName, message.Text);
            return FileName;
        }

        private string GetFilePath(string destinyDir, IStructuredMessage message) {
            string Extension = null;
            if (message.Content == ContentsTypes.Text) {
                Extension = "txt";
            }
            if (message.Content == ContentsTypes.Html) {
                Extension = "html";
            }
            string ValidTitle = message.Title;
            foreach (char InvalidChar in _Path.GetInvalidFileNameChars()) {
                ValidTitle = ValidTitle.Replace(InvalidChar, '_');
            }

            string FullPath = _Path.Combine(destinyDir, ValidTitle + "." + Extension);

            return FullPath;
        }
    }
}