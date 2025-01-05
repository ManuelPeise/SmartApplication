namespace Shared.Models.Response
{
    public class ApiResponse<T> where T : class
    {
        public int StatusCode { get; set; }
        public T? Data { get; set; }
    }
}
