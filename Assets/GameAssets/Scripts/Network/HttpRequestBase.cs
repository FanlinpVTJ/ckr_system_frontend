using System;

namespace CkrSystem.Network
{
    public abstract class HttpRequestBase
    {
        public Guid Id { get; }
        public RequestScopeType ScopeType { get; }
        public string Url { get; }

        protected HttpRequestBase(RequestScopeType scopeType, string url)
        {
            Id = Guid.NewGuid();
            ScopeType = scopeType;
            Url = url;
        }

        public abstract void CompleteSuccess(IJsonConverter jsonConverter, string responseBody, long statusCode);
        public abstract void CompleteFailure(long statusCode, string errorMessage);
        public abstract void CompleteCancellation();
    }
}
