using System.Text.Json.Serialization;

namespace ShopingCartAPI.Common.Results
{
    public class Result<T>
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

        public static Result<T> Success(T value)
        {
            return new Result<T>(value, true, string.Empty);
        }

        public static Result<T> Fail(string message)
        {
            return new Result<T>(default, false, message);
        }

    }
}
