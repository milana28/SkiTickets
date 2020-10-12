namespace SkiTickets.Utils.Responses
{
    public class OkResponse<T>
    {
        public T Data { set; get; }

        public OkResponse(T response)
        {
            Data = response;
        }
    }
}