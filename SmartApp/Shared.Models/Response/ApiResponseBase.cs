namespace Shared.Models.Response
{
    public class ApiResponseBase<T> where T : class
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
    }
}
