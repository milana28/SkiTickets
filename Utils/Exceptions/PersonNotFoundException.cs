using System;

namespace SkiTickets.Utils.Exceptions
{
    public class PersonNotFoundException : Exception
    {
        public PersonNotFoundException(string message)
        :base (message) { }
    }
}