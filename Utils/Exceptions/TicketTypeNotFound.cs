using System;

namespace SkiTickets.Utils.Exceptions
{
    public class TicketTypeNotFound : Exception
    {
        public TicketTypeNotFound(string message)
            :base(message) { }
    }
}