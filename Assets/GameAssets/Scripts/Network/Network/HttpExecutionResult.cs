namespace FeaturedClicker.Network
{
    public class HttpExecutionResult
    {
        public bool IsSuccess { get; }
        public bool IsCancelled { get; }
        public long StatusCode { get; }
        public string ResponseBody { get; }
        public string ErrorMessage { get; }

        public HttpExecutionResult(bool isSuccess, bool isCancelled, long statusCode, string responseBody, string errorMessage)
        {
            IsSuccess = isSuccess;
            IsCancelled = isCancelled;
            StatusCode = statusCode;
            ResponseBody = responseBody;
            ErrorMessage = errorMessage;
        }
    }
}
