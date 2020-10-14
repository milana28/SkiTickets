namespace SkiTickets.Utils.Responses
{
    public class OkResponse<T>
    {
        public T Data { get; }

        public OkResponse(T response)
        {
            Data = response;
        }
    }
}