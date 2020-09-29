using System;

namespace SkiTickets.Utils.Exceptions
{
    public class AgesNotMatchingException : Exception
    {
        public AgesNotMatchingException(string message) 
            :base(message) { }
    }
}