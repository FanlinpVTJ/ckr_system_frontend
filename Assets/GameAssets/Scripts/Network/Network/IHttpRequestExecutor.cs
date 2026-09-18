using System.Threading;
using Cysharp.Threading.Tasks;

namespace FeaturedClicker.Network
{
    public interface IHttpRequestExecutor
    {
        UniTask<HttpExecutionResult> ExecuteAsync(HttpRequestBase request, CancellationToken cancellationToken);
    }
}
