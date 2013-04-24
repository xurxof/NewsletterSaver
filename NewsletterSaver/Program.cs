using System;
using System.Linq;
using SystemWrapper;
using SystemWrapper.IO;

namespace NewsletterSaver {
    internal class Program {
        private static void Main(string[] args) {
            try {
                Config Cfg = new Config(args[0], new FileWrap());
                InternalConfigManager CfgInternal = new InternalConfigManager(new FileWrap(), new ApllicationWrap());
                var MaxDate = CfgInternal.GetMaxDate();
                MailClient Client = new MailClient(Cfg.ImapUseSSL, Cfg.ImapHost, Cfg.ImapUser, Cfg.ImapPass, Cfg.ImapPort);
                MailFilter Filter = new MailFilter(Cfg.FromFilter.ToArray());
                MailReader Reader = new MailReader(Client, Filter, new DateTimeWrap());
                var Mails = Reader.GetUnreadMails(MaxDate);
                MessageSaver Saver = new MessageSaver(new FileWrap(), Cfg.SavePath, new PathWrap(), new HtmlRemoteToLocalConverter(new WebFacade()));
                var Enumerable = Mails as IMessage[] ?? Mails.ToArray();
                Saver.Save(Enumerable);


                DateTime? NewMaxDate = Enumerable.Max(m => m.Date);
                if (NewMaxDate.HasValue) {
                    CfgInternal.SaveDate(NewMaxDate.Value);
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
            }
        }
    }
}