using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace FeaturedClicker.Network
{
    public class HttpRequest<TResponse> : HttpRequestBase
    {
        private readonly UniTaskCompletionSource<HttpResponse<TResponse>> _completionSource = new UniTaskCompletionSource<HttpResponse<TResponse>>();

        public HttpRequest(RequestScopeType scopeType, string url, HttpMethodType methodType = HttpMethodType.Get, string requestBody = "", List<HttpHeader> headers = null)
            : base(scopeType, url, methodType, requestBody, headers ?? new List<HttpHeader>())
        {
        }

        public UniTask<HttpResponse<TResponse>> GetResponseAsync()
        {
            UniTask<HttpResponse<TResponse>> responseTask = _completionSource.Task;

            return responseTask;
        }

        public override void CompleteSuccess(IJsonConverter jsonConverter, string responseBody, long statusCode)
        {
            TResponse data;
            string errorMessage;

            if (!jsonConverter.TryDeserialize(responseBody, out data, out errorMessage))
            {
                CompleteFailure(statusCode, errorMessage);

                return;
            }

            HttpResponse<TResponse> response = new HttpResponse<TResponse>(true, false, statusCode, data, string.Empty);
            _completionSource.TrySetResult(response);
        }

        public override void CompleteFailure(long statusCode, string errorMessage)
        {
            HttpResponse<TResponse> response = new HttpResponse<TResponse>(false, false, statusCode, default, errorMessage);
            _completionSource.TrySetResult(response);
        }

        public override void CompleteCancellation()
        {
            HttpResponse<TResponse> response = new HttpResponse<TResponse>(false, true, 0, default, "Request was cancelled.");
            _completionSource.TrySetResult(response);
        }
    }
}
