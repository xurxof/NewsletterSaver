using System.IO;
using System.Net;

namespace NewsletterSaver {
    sealed class WebFacade :IWebFacade{
        public byte[] GetBinaryRemoteFile(string url) {

            WebRequest req = WebRequest.Create(url);
            using (WebResponse resp = req.GetResponse()) {
                using (Stream stream = resp.GetResponseStream()) {
                    byte[] buffer = new byte[resp.ContentLength];
                    MemoryStream fs = new MemoryStream();
                    
                    stream.Read(buffer, 0, (int)resp.ContentLength);
                    fs.Write(buffer, 0, (int)resp.ContentLength);
                    fs.Close();
                    return buffer;
                }
            }
            
        }
    }
}
