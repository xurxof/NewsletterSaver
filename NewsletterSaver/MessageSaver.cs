using System;
using System.Collections.Generic;
using System.IO;
using SystemWrapper.IO;
using NLog;


namespace NewsletterSaver {
    internal sealed class MessageSaver {
        private static readonly Logger _Logger = LogManager.GetCurrentClassLogger();
        private readonly IFileWrap _File;

        private readonly string _OutpuDirectory;
        private readonly IPathWrap _PathWrap;
        private readonly HtmlRemoteToLocalConverter _HtmlConverter;


        public MessageSaver(IFileWrap file, string outpuDirectory, IPathWrap pathWrap, HtmlRemoteToLocalConverter htmlConverter) {
            _File = file;
            _OutpuDirectory = outpuDirectory;
            _PathWrap = pathWrap;
            _HtmlConverter = htmlConverter;
        }

        private string Save(IMessage message) {
            
            if (message == null) {
                _Logger.Warn("A 'null' message was attemped to be saved!");
                return null;
            }
            
            string FileName = GetOutputFilePath(message);
            if (_File.Exists(FileName)) {
                _Logger.Warn("File {0} already exists. Mesagge will not be saved", FileName);
                return FileName;
            }
            if (message.IsHtml) {
                SaveHtmlMessage(message, FileName);
            }
            else {
                SaveTextMessage(message, FileName);
            }
            return FileName;
        }

        private void SaveTextMessage(IMessage message, string FileName) {
            _Logger.Info("Saving {0}", FileName);
            _File.WriteAllText(FileName, message.Text);
        }

        private void SaveHtmlMessage(IMessage message, string FileName) {
            _Logger.Info("Saving {0} ", FileName);
            IInMemoryDoc Converted = _HtmlConverter.Convert(message.Text, GetValidSubDirectory(message.Title) + "_files", _OutpuDirectory);
            _File.WriteAllText(FileName, Converted.Text);
            foreach (BinaryReference BinaryReference in Converted.BinaryReferences) {
                SaveBinaryReference(BinaryReference);
            }
        }

        private void SaveBinaryReference(BinaryReference binaryReference) {
            string LocalDirectory = _PathWrap.GetDirectoryName(binaryReference.NewLocalLink);
            try {
                if (!Directory.Exists(LocalDirectory)) {
                    Directory.CreateDirectory(LocalDirectory);
                }
                _File.WriteAllBytes(binaryReference.NewLocalLink, binaryReference.BinaryValue);
            }
            catch (Exception ex) {
                _Logger.Error(String.Format("Exception saving binary file {0}. The exception was ignored.", binaryReference.NewLocalLink),ex);
                // all the esceptions like file existent, illegal name, etc. are passed; 
                // binary data is not prioritary
            }
        }


        private string GetOutputFilePath(IMessage message) {
            string Extension;
            Extension = message.IsHtml ? "html" : "txt";

            return GetValidDirectoryName(message.Title) + "." + Extension;
        }

        private string GetValidDirectoryName(string messageSubject) {
            string ValidTitle = GetValidSubDirectory(messageSubject);
            return _PathWrap.Combine(_OutpuDirectory, ValidTitle);
        }

        private string GetValidSubDirectory(string messageSubject) {
            string ValidSubdirectory = messageSubject;
            foreach (char InvalidChar in _PathWrap.GetInvalidFileNameChars()) {
                ValidSubdirectory = ValidSubdirectory.Replace(InvalidChar, '_');
            }
            return ValidSubdirectory;
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