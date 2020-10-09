using System;

namespace SkiTickets.Utils.Exceptions
{
    public class TicketNotFoundException : Exception
    {
        public TicketNotFoundException(string message)
            :base(message) { }
    }
}