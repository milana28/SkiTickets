using System;

namespace SkiTickets.Utils.Exceptions
{
    public class AgeBadRequestException : Exception
    {
        public AgeBadRequestException(string message)
            :base(message) { }
    }
}