using System.Collections.Generic;
using SystemWrapper.IO;

namespace NewsletterSaver {
    internal sealed class MessageSaver {
        private readonly IFileWrap _File;

        private readonly string _DestinyPath;
        private readonly IPathWrap _Path;
        private readonly HtmlRemoteToLocalConverter _HtmlConverter;


        public MessageSaver(IFileWrap file, string destinyPath, IPathWrap path, HtmlRemoteToLocalConverter htmlConverter) {
            _File = file;
            _DestinyPath = destinyPath;
            _Path = path;
            _HtmlConverter = htmlConverter;
        }

        private string Save(IMessage message) {
            if (message == null) {
                return null;
            }
            string FileName = GetFilePath(message);
            if (_File.Exists(FileName))
                return FileName;
            if (message.IsHtml) {
                IInMemoryDoc Converted = _HtmlConverter.Convert(message.Text, GetValidDirectoryName(message.Title)+"_files");
                _File.WriteAllText(FileName, Converted.Text);
                foreach (BinaryReference BinaryReference in Converted.BinaryReferences) {
                    SaveBinaryReference(BinaryReference);
                }
            }
            else {
                _File.WriteAllText(FileName, message.Text);
            }
            return FileName;
        }

        private void SaveBinaryReference(BinaryReference BinaryReference) {
            var LocalDirectory = _Path.GetDirectoryName(BinaryReference.NewLocalLink);
            if (!System.IO.Directory.Exists(LocalDirectory)) {
                System.IO.Directory.CreateDirectory(LocalDirectory);
            }
            _File.WriteAllBytes(BinaryReference.NewLocalLink, BinaryReference.BinaryValue);
        }


        private string GetFilePath(IMessage message) {
            string Extension ;
            Extension = message.IsHtml ? "html" : "txt";

            return GetValidDirectoryName(message.Title) + "." + Extension;
        }

        private string GetValidDirectoryName(string messageSubject) {
            string ValidTitle = messageSubject;
            foreach (char InvalidChar in _Path.GetInvalidFileNameChars()) {
                ValidTitle = ValidTitle.Replace(InvalidChar, '_');
            }
            return _Path.Combine(_DestinyPath, ValidTitle);
        }

        internal string[] Save(IEnumerable<IMessage> mails) {
            if (mails == null) {
                return new string[] {};
            }
            var FileNames = new List<string>();
            foreach (IMessage Message in mails) {
                FileNames.Add(Save(Message));
            }
            return FileNames.ToArray();
        }
    }
}