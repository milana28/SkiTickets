using System;

namespace SkiTickets.Utils.Exceptions
{
    public class TicketPurchaseNotFound : Exception
    {
        public TicketPurchaseNotFound(string message)
            :base(message) { }
    }
}