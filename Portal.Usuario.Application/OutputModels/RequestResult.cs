namespace Portal.Usuario.Application.OutputModels
{
    public class RequestResult<T>
    {
        public RequestResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public RequestResult(bool success, string message, T data)
        {
            Success = success;
            Message = message;
            Data = data;
        }
        public bool Success { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }
    }
}
