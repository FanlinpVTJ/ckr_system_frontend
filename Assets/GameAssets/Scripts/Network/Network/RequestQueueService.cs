using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace FeaturedClicker.Network
{
    public class RequestQueueService
    {
        private readonly IJsonConverter _jsonConverter;
        private readonly IHttpRequestExecutor _httpRequestExecutor;
        private readonly List<HttpRequestBase> _queuedRequests = new List<HttpRequestBase>();

        private HttpRequestBase _activeRequest;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isProcessing;

        public RequestQueueService(IJsonConverter jsonConverter, IHttpRequestExecutor httpRequestExecutor)
        {
            _jsonConverter = jsonConverter;
            _httpRequestExecutor = httpRequestExecutor;
        }

        public UniTask<HttpResponse<TResponse>> Enqueue<TResponse>(HttpRequest<TResponse> request)
        {
            _queuedRequests.Add(request);
            StartProcessing();

            return request.GetResponseAsync();
        }

        public void CancelRequests(RequestScopeType scopeType)
        {
            for (int i = _queuedRequests.Count - 1; i >= 0; i--)
            {
                HttpRequestBase request = _queuedRequests[i];

                if (request.ScopeType != scopeType)
                {
                    continue;
                }

                _queuedRequests.RemoveAt(i);
                request.CompleteCancellation();
            }

            if (_activeRequest != null && _activeRequest.ScopeType == scopeType)
            {
                _cancellationTokenSource.Cancel();
            }
        }

        public bool CancelRequest(Guid requestId)
        {
            for (int i = _queuedRequests.Count - 1; i >= 0; i--)
            {
                HttpRequestBase request = _queuedRequests[i];

                if (request.Id != requestId)
                {
                    continue;
                }

                _queuedRequests.RemoveAt(i);
                request.CompleteCancellation();

                return true;
            }

            if (_activeRequest != null && _activeRequest.Id == requestId)
            {
                _cancellationTokenSource.Cancel();

                return true;
            }

            return false;
        }

        private void StartProcessing()
        {
            if (_isProcessing)
            {
                return;
            }

            ProcessQueueAsync().Forget();
        }

        private async UniTaskVoid ProcessQueueAsync()
        {
            _isProcessing = true;

            while (_queuedRequests.Count > 0)
            {
                _activeRequest = _queuedRequests[0];
                _queuedRequests.RemoveAt(0);
                _cancellationTokenSource = new CancellationTokenSource();

                HttpExecutionResult result = await _httpRequestExecutor.ExecuteAsync(_activeRequest, _cancellationTokenSource.Token);
                CompleteRequest(_activeRequest, result);

                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
                _activeRequest = null;
            }

            _isProcessing = false;
        }

        private void CompleteRequest(HttpRequestBase request, HttpExecutionResult result)
        {
            if (result.IsCancelled)
            {
                request.CompleteCancellation();

                return;
            }

            if (!result.IsSuccess)
            {
                request.CompleteFailure(result.StatusCode, result.ErrorMessage);

                return;
            }

            request.CompleteSuccess(_jsonConverter, result.ResponseBody, result.StatusCode);
        }
    }
}
