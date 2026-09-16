using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

namespace CkrSystem.Network
{
    public class RequestQueueService
    {
        private readonly IJsonConverter _jsonConverter;
        private readonly List<HttpRequestBase> _queuedRequests = new List<HttpRequestBase>();

        private HttpRequestBase _activeRequest;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isProcessing;

        public RequestQueueService(IJsonConverter jsonConverter)
        {
            _jsonConverter = jsonConverter;
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

                await SendAsync(_activeRequest, _cancellationTokenSource.Token);

                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
                _activeRequest = null;
            }

            _isProcessing = false;
        }

        private async UniTask SendAsync(HttpRequestBase request, CancellationToken cancellationToken)
        {
            using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(request.Url))
            {
                try
                {
                    await unityWebRequest.SendWebRequest().ToUniTask(cancellationToken: cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    unityWebRequest.Abort();
                    request.CompleteCancellation();

                    return;
                }
                catch (UnityWebRequestException)
                {
                    request.CompleteFailure(unityWebRequest.responseCode, unityWebRequest.error);

                    return;
                }

                if (unityWebRequest.result != UnityWebRequest.Result.Success)
                {
                    request.CompleteFailure(unityWebRequest.responseCode, unityWebRequest.error);

                    return;
                }

                request.CompleteSuccess(_jsonConverter, unityWebRequest.downloadHandler.text, unityWebRequest.responseCode);
            }
        }
    }
}
