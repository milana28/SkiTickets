using System;

namespace SkiTickets.Utils.Exceptions
{
    public class PersonBadRequestException : Exception
    {
        public PersonBadRequestException(string message)
        :base (message) { }
    }
}