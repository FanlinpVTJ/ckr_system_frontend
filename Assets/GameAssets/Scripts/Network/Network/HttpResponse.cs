namespace FeaturedClicker.Network
{
    public class HttpResponse<TResponse>
    {
        public bool IsSuccess { get; }
        public bool IsCancelled { get; }
        public long StatusCode { get; }
        public TResponse Data { get; }
        public string ErrorMessage { get; }

        public HttpResponse(bool isSuccess, bool isCancelled, long statusCode, TResponse data, string errorMessage)
        {
            IsSuccess = isSuccess;
            IsCancelled = isCancelled;
            StatusCode = statusCode;
            Data = data;
            ErrorMessage = errorMessage;
        }
    }
}
