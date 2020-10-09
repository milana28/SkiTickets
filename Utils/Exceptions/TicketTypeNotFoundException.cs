using System;

namespace SkiTickets.Utils.Exceptions
{
    public class TicketTypeNotFoundException : Exception
    {
        public TicketTypeNotFoundException(string message)
            :base(message) { }
    }
}