namespace BloodDonationAppUserService.Response
{
    public class Response<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }= string.Empty;
        public int StatusCode { get; set; }

        public static Response<T> SuccessResponse(T data, int statusCode = 200)
            => new Response<T> { Data = data, Success = true, StatusCode = statusCode };

        public static Response<T> FailResponse(string message, int statusCode)
            => new Response<T> { Data = default, Success = false, Message = message, StatusCode = statusCode };
    }
}
