using System;
using System.Collections.Generic;

namespace NewsletterSaver {
    /// <summary>
    /// An inteface form encapsulate a external library.
    /// </summary>
    public interface IMailClient {
        /// <summary>
        /// Get messages recived after a datetime (server time)
        /// </summary>
        /// <param name="minDateTime">Minimum datetime of messages retrieved.</param>
        /// <returns>Set of recived messages.</returns>
        IEnumerable<IMessage> GetMessagesAfter(DateTime minDateTime);

    }
}