using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using NUnit.Framework;

namespace CkrSystem.Network.Tests
{
    public class RequestQueueServiceTests
    {
        [Test]
        public async Task Enqueue_ShouldReturnDeserializedResponse()
        {
            FakeHttpRequestExecutor executor = new FakeHttpRequestExecutor();
            executor.Results.Enqueue(new HttpExecutionResult(true, false, 200, "{\"Value\":5}", string.Empty));
            RequestQueueService service = new RequestQueueService(new JsonConverter(), executor);
            HttpRequest<TestResponse> request = new HttpRequest<TestResponse>(RequestScopeType.Weather, "https://example.com/weather");

            HttpResponse<TestResponse> response = await service.Enqueue(request);

            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual(5, response.Data.Value);
        }

        [Test]
        public async Task Enqueue_ShouldPreserveRequestOrder()
        {
            FakeHttpRequestExecutor executor = new FakeHttpRequestExecutor();
            executor.Results.Enqueue(new HttpExecutionResult(true, false, 200, "{\"Value\":1}", string.Empty));
            executor.Results.Enqueue(new HttpExecutionResult(true, false, 200, "{\"Value\":2}", string.Empty));
            RequestQueueService service = new RequestQueueService(new JsonConverter(), executor);

            await service.Enqueue(new HttpRequest<TestResponse>(RequestScopeType.Weather, "first"));
            await service.Enqueue(new HttpRequest<TestResponse>(RequestScopeType.DogBreeds, "second"));

            CollectionAssert.AreEqual(new[] { "first", "second" }, executor.ExecutedUrls);
        }

        [Test]
        public async Task Enqueue_ShouldReturnHttpError()
        {
            FakeHttpRequestExecutor executor = new FakeHttpRequestExecutor();
            executor.Results.Enqueue(new HttpExecutionResult(false, false, 503, string.Empty, "Unavailable"));
            RequestQueueService service = new RequestQueueService(new JsonConverter(), executor);

            HttpResponse<TestResponse> response = await service.Enqueue(new HttpRequest<TestResponse>(RequestScopeType.Weather, "weather"));

            Assert.IsFalse(response.IsSuccess);
            Assert.AreEqual(503, response.StatusCode);
        }

        [Test]
        public async Task Enqueue_ShouldReturnJsonError()
        {
            FakeHttpRequestExecutor executor = new FakeHttpRequestExecutor();
            executor.Results.Enqueue(new HttpExecutionResult(true, false, 200, "invalid", string.Empty));
            RequestQueueService service = new RequestQueueService(new JsonConverter(), executor);

            HttpResponse<TestResponse> response = await service.Enqueue(new HttpRequest<TestResponse>(RequestScopeType.Weather, "weather"));

            Assert.IsFalse(response.IsSuccess);
            Assert.IsFalse(string.IsNullOrEmpty(response.ErrorMessage));
        }

        [Test]
        public async Task CancelRequest_ShouldRemoveQueuedRequest()
        {
            PendingHttpRequestExecutor executor = new PendingHttpRequestExecutor();
            RequestQueueService service = new RequestQueueService(new JsonConverter(), executor);
            HttpRequest<TestResponse> activeRequest = new HttpRequest<TestResponse>(RequestScopeType.Weather, "active");
            HttpRequest<TestResponse> queuedRequest = new HttpRequest<TestResponse>(RequestScopeType.DogBreeds, "queued");

            UniTask<HttpResponse<TestResponse>> activeTask = service.Enqueue(activeRequest);
            UniTask<HttpResponse<TestResponse>> queuedTask = service.Enqueue(queuedRequest);
            bool isCancelled = service.CancelRequest(queuedRequest.Id);
            HttpResponse<TestResponse> queuedResponse = await queuedTask;

            Assert.IsTrue(isCancelled);
            Assert.IsTrue(queuedResponse.IsCancelled);

            executor.Complete(new HttpExecutionResult(true, false, 200, "{\"Value\":1}", string.Empty));
            await activeTask;
        }

        private class TestResponse
        {
            public int Value { get; set; }
        }

        private class FakeHttpRequestExecutor : IHttpRequestExecutor
        {
            public Queue<HttpExecutionResult> Results { get; } = new Queue<HttpExecutionResult>();
            public List<string> ExecutedUrls { get; } = new List<string>();

            public UniTask<HttpExecutionResult> ExecuteAsync(HttpRequestBase request, CancellationToken cancellationToken)
            {
                ExecutedUrls.Add(request.Url);
                HttpExecutionResult result = Results.Dequeue();

                return UniTask.FromResult(result);
            }
        }

        private class PendingHttpRequestExecutor : IHttpRequestExecutor
        {
            private readonly UniTaskCompletionSource<HttpExecutionResult> _completionSource = new UniTaskCompletionSource<HttpExecutionResult>();

            public UniTask<HttpExecutionResult> ExecuteAsync(HttpRequestBase request, CancellationToken cancellationToken)
            {
                return _completionSource.Task;
            }

            public void Complete(HttpExecutionResult result)
            {
                _completionSource.TrySetResult(result);
            }
        }
    }
}
