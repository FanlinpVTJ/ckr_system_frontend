using Cysharp.Threading.Tasks;
using CkrSystem.Network;

namespace CkrSystem.Network.Features.Dogs
{
    public class DogApiClient
    {
        private readonly DogEndpointConfig _endpointConfig;
        private readonly RequestQueueService _requestQueueService;

        public DogApiClient(DogEndpointConfig endpointConfig, RequestQueueService requestQueueService)
        {
            _endpointConfig = endpointConfig;
            _requestQueueService = requestQueueService;
        }

        public UniTask<HttpResponse<DogBreedsResponse>> GetBreedsAsync()
        {
            string url = $"{_endpointConfig.BaseUrl}/breeds?page[size]=10";
            HttpRequest<DogBreedsResponse> request = new HttpRequest<DogBreedsResponse>(RequestScopeType.DogBreeds, url);
            UniTask<HttpResponse<DogBreedsResponse>> responseTask = _requestQueueService.Enqueue(request);

            return responseTask;
        }

        public UniTask<HttpResponse<DogBreedResponse>> GetBreedAsync(string breedId)
        {
            string url = $"{_endpointConfig.BaseUrl}/breeds/{breedId}";
            HttpRequest<DogBreedResponse> request = new HttpRequest<DogBreedResponse>(RequestScopeType.DogBreedDetails, url);
            UniTask<HttpResponse<DogBreedResponse>> responseTask = _requestQueueService.Enqueue(request);

            return responseTask;
        }
    }
}
