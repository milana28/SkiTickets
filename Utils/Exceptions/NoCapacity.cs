using System;

namespace SkiTickets.Utils.Exceptions
{
    public class NoCapacity : Exception
    {
        public NoCapacity(string message)
            :base (message) { }
    }
}