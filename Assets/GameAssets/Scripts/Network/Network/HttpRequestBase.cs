using System;
using System.Collections.Generic;

namespace FeaturedClicker.Network
{
    public abstract class HttpRequestBase
    {
        public Guid Id { get; }
        public RequestScopeType ScopeType { get; }
        public string Url { get; }
        public HttpMethodType MethodType { get; }
        public string RequestBody { get; }
        public List<HttpHeader> Headers { get; }

        protected HttpRequestBase(RequestScopeType scopeType, string url, HttpMethodType methodType, string requestBody, List<HttpHeader> headers)
        {
            Id = Guid.NewGuid();
            ScopeType = scopeType;
            Url = url;
            MethodType = methodType;
            RequestBody = requestBody;
            Headers = headers;
        }

        public abstract void CompleteSuccess(IJsonConverter jsonConverter, string responseBody, long statusCode);
        public abstract void CompleteFailure(long statusCode, string errorMessage);
        public abstract void CompleteCancellation();
    }
}
