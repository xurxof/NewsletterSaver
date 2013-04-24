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
            _MaxDateFilePath = Path.Combine(Path.GetDirectoryName(directoryWrap.ExecutablePath), InternalConfigFilename);
        }


        internal DateTime? GetMaxDate() {
            if (!_FileWrap.Exists(_MaxDateFilePath)) {
                return null;
            }
            var Line = _FileWrap.ReadAllLines(_MaxDateFilePath)[0];

            return DateTime.Parse(Line, System.Globalization.CultureInfo.InvariantCulture);
        }

        internal bool SaveDate(DateTime maxDate) {

            _FileWrap.WriteAllText(_MaxDateFilePath, maxDate.ToString(System.Globalization.CultureInfo.InvariantCulture));
            return true;
        }
    }
}