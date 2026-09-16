using System;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

namespace CkrSystem.Network
{
    public class UnityWebRequestExecutor : IHttpRequestExecutor
    {
        public async UniTask<HttpExecutionResult> ExecuteAsync(HttpRequestBase request, CancellationToken cancellationToken)
        {
            using (UnityWebRequest unityWebRequest = new UnityWebRequest(request.Url, request.MethodType.ToString().ToUpperInvariant()))
            {
                unityWebRequest.downloadHandler = new DownloadHandlerBuffer();

                foreach (HttpHeader header in request.Headers)
                {
                    unityWebRequest.SetRequestHeader(header.Name, header.Value);
                }

                if (!string.IsNullOrWhiteSpace(request.RequestBody))
                {
                    unityWebRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(request.RequestBody));
                    unityWebRequest.SetRequestHeader("Content-Type", "application/json");
                }

                try
                {
                    await unityWebRequest.SendWebRequest().ToUniTask(cancellationToken: cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    unityWebRequest.Abort();
                    HttpExecutionResult cancelledResult = new HttpExecutionResult(false, true, 0, string.Empty, "Request was cancelled.");

                    return cancelledResult;
                }
                catch (UnityWebRequestException)
                {
                    HttpExecutionResult failedResult = new HttpExecutionResult(false, false, unityWebRequest.responseCode, string.Empty, unityWebRequest.error);

                    return failedResult;
                }

                bool isSuccess = unityWebRequest.result == UnityWebRequest.Result.Success;
                HttpExecutionResult result = new HttpExecutionResult(isSuccess, false, unityWebRequest.responseCode, unityWebRequest.downloadHandler.text, unityWebRequest.error);

                return result;
            }
        }
    }
}
