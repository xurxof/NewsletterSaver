using System;
using System.Collections.Generic;
using SystemWrapper.IO;

namespace NewsletterSaver {
    internal sealed class Config {
        private readonly List<string> _FromFilter;
        private string _ImapPass;
        private string _SavePath;
        private string _ImapUser;
        private int _ImapPort;
        private bool _ImapUseSSL;
        private string _ImapHost;


        public Config(string fileName, IFileWrap fileReader) {
            _FromFilter = new List<string>();
            if(!fileReader.Exists(fileName)) {
                var DefaultConfigStrings = new[] {"imapUser:pepejuan@gmail.com", "imapPass:password", "imapHost:imap.gmail.com", "imapPort:993", "imapUseSSL:true", @"savePath:c:\mails\", "fromFilter:uno@blabla.com", "fromFilter:dos@blabla.com", "fromFilter:tres@blabla.com"};
                fileReader.WriteAllLines(fileName, DefaultConfigStrings);
            }
            var Lines = fileReader.ReadAllLines(fileName);
            foreach (string Line in Lines) {
                if (string.IsNullOrWhiteSpace(Line)) continue;
                var KeyValue = ExtractKeyValue(Line);
                SetPropertyValue(KeyValue.Item1, KeyValue.Item2);
            }
        }


        private static Tuple<string, string> ExtractKeyValue(string linea) {
            var SplittedLine = linea.Split(':');

            string Key = SplittedLine[0];
            string Value = linea.Substring(Key.Length + 1);
            var Tuple = new Tuple<string, string>(Key, Value);
            return Tuple;
        }

        private void SetPropertyValue(string key, string value) {
            switch (key) {
                case "imapPass":
                    _ImapPass = value;
                    break;
                case "imapPort":
                    _ImapPort = int.Parse(value);
                    break;
                case "savePath":
                    _SavePath = value;
                    break;
                case "imapUser":
                    _ImapUser = value;
                    break;
                case "imapHost":
                    _ImapHost = value;
                    break;
                case "imapUseSSL":
                    _ImapUseSSL = bool.Parse(value);
                    break;
                case "fromFilter":
                    _FromFilter.Add(value);
                    break;
            }
        }

        public string ImapPass {
            get {
                return _ImapPass;
            }
        }

        public int ImapPort {
            get {
                return _ImapPort;
            }
        }

        public string SavePath {
            get {
                return _SavePath;
            }
        }

        public IEnumerable<string> FromFilter {
            get {
                return _FromFilter;
            }
        }

        public string ImapUser {
            get {
                return _ImapUser;
            }
        }

        public string ImapHost {
            get {
                return _ImapHost;
            }
        }

        public bool ImapUseSSL {
            get {
                return _ImapUseSSL;
            }
        }
    }
}