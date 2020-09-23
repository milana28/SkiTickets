using System;

namespace SkiTickets.Utils.Exceptions
{
    public class AgeNotFoundException : Exception
    {
        public AgeNotFoundException(string message)
            :base(message) { }
    }
}