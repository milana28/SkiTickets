using System;

namespace SkiTickets.Utils.Exceptions
{
    public class TicketNotFound : Exception
    {
        public TicketNotFound(string message)
            :base(message) { }
    }
}