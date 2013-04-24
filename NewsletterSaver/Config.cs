using System;
using System.Collections.Generic;
using SystemWrapper.IO;

namespace NewsletterSaver {
    internal sealed class Config {
        private readonly List<string> _FromFilter;
        private string _PopPass;
        private string _SavePath;
        private string _PopUser;
        private int _PopPort;
        private bool _PopUseSSL;
        private string _PopHost;


        public Config(string fileName, IFileWrap fileReader) {
            _FromFilter = new List<string>();
            var Lineas = fileReader.ReadAllLines(fileName);
            foreach (string Linea in Lineas) {
                var KeyValue = ExtractKeyValue(Linea);
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
                    _PopPass = value;
                    break;
                case "imapPort":
                    _PopPort = int.Parse(value);
                    break;
                case "savePath":
                    _SavePath = value;
                    break;
                case "imapUser":
                    _PopUser = value;
                    break;
                case "imapHost":
                    _PopHost = value;
                    break;
                case "imapUseSSL":
                    _PopUseSSL = bool.Parse(value);
                    break;
                case "fromFilter":
                    _FromFilter.Add(value);
                    break;
            }
        }

        public string PopPass {
            get {
                return _PopPass;
            }
        }

        public int PopPort {
            get {
                return _PopPort;
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

        public string PopUser {
            get {
                return _PopUser;
            }
        }

        public string PopHost {
            get {
                return _PopHost;
            }
        }

        public bool PopUseSSL {
            get {
                return _PopUseSSL;
            }
        }
    }
}