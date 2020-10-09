using System;

namespace SkiTickets.Utils.Exceptions
{
    public class TicketPurchaseNotFoundException : Exception
    {
        public TicketPurchaseNotFoundException(string message)
            :base(message) { }
    }
}