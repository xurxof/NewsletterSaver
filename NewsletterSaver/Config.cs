using System.Collections.Generic;
using SystemWrapper.IO;

namespace NewsletterSaver {
    internal sealed class Config {

        private readonly List<string> _FromFilter;
        private readonly string _PopPass;
        private readonly string _SavePath;
        private readonly string _PopUser;
        private readonly int _PopPort;
        private readonly bool _PopUseSSL;
        private readonly string _PopHost;


        public Config(string fileName, IFileWrap fileReader) {
            _FromFilter = new List<string>();
            var Lineas = fileReader.ReadAllLines(fileName);
            foreach (var Linea in Lineas) {
                var SplittedLine = Linea.Split(':');
                string Key = SplittedLine[0];
                string Value= Linea.Substring(Key.Length+1);
                switch  (Key)
                {
                    case "popPass":
                        _PopPass= Value;
                        break;
                    case "popPort":
                        _PopPort = int.Parse(Value);
                        break;
                    case "savePath":
                        _SavePath = Value;
                        break;
                    case "popUser":
                        _PopUser = Value;
                        break;
                    case "popHost":
                        _PopHost = Value;
                        break;
                    case "popUseSSL":
                        _PopUseSSL = bool.Parse (Value);
                        break;
                    case "fromFilter":
                        _FromFilter.Add(Value);
                        break;
                }
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