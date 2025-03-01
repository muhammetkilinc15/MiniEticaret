namespace OrderAPI.Commons.Result
{
    public sealed class Result<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public Result(T data, bool isSuccess, string message)
        {
            Data = data;
            IsSuccess = isSuccess;
            Message = message;
        }
        public Result()
        {
            
        }
        public static Result<T> Success(T data, string message = "Success")
        {
            return new Result<T>(data, true, message);
        }
        public static Result<T> Failure(string message = "Failure")
        {
            return new Result<T>(default, false, message);
        }
    }
}
