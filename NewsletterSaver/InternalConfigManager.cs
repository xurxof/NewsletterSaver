using System;
using System.IO;
using SystemWrapper.IO;

namespace NewsletterSaver {
    internal sealed class InternalConfigManager {
        private readonly IFileWrap _FileWrap;
        private readonly string _MaxDateFilePath;
        private const string InternalConfigFilename = "maxdate.txt";

        public InternalConfigManager(IFileWrap fileWrap, IApplicationWrap directoryWrap) {
            _FileWrap = fileWrap;
            _MaxDateFilePath = Path.Combine(directoryWrap.ExecutablePath, InternalConfigFilename);
        }


        internal DateTime? GetMaxDate() {
            if (!_FileWrap.Exists(_MaxDateFilePath)) {
                return null;
            }
            return DateTime.Parse(_FileWrap.ReadAllLines(_MaxDateFilePath)[0]);
        }
    }
}