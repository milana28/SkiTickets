using System.Collections.Generic;
using System.Linq;
using SkiTickets.Utils.Models;

namespace SkiTickets.Utils
{
    public class CreateErrorList
    {
        public static object MapErrorObject(List<Error>? errors)
        {
            var errorObject = new Dictionary<string, List<string>>();
            errors?.ForEach(e =>
            {
                var messageList = new List<string> {e.Message};
                errorObject.Add(e.Attribute, messageList);
            });

            return errorObject;
        }

        public static List<Error> GetErrors(IEnumerable<string>? attributes, string message)
        {
            return attributes?.Select(attribute => new Error() {Attribute = attribute, Message = message}).ToList();
        }
    }
}