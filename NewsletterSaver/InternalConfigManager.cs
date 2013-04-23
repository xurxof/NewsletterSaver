using System;
using System.IO;
using SystemWrapper.IO;

namespace NewsletterSaver {
    internal sealed class InternalConfigManager {
        private readonly IFileWrap _FileWrap;
        private readonly IApplicationWrap _DirectoryWrap;

        public InternalConfigManager(IFileWrap fileWrap, IApplicationWrap directoryWrap) {
            _FileWrap = fileWrap;
            _DirectoryWrap = directoryWrap;
        }

        internal DateTime? GetMaxDate() {
            var MaxDateFilePath = Path.Combine(_DirectoryWrap.ExecutablePath, "maxdate.txt");
            if (!_FileWrap.Exists(MaxDateFilePath)) return null;
            return DateTime.Parse(_FileWrap.ReadAllLines(MaxDateFilePath)[0]);
        }
    }
}