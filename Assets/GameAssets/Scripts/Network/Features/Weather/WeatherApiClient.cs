using Cysharp.Threading.Tasks;
using FeaturedClicker.Network;

namespace FeaturedClicker.Network.Features.Weather
{
    public class WeatherApiClient
    {
        private readonly WeatherEndpointConfig _endpointConfig;
        private readonly RequestQueueService _requestQueueService;

        public WeatherApiClient(WeatherEndpointConfig endpointConfig, RequestQueueService requestQueueService)
        {
            _endpointConfig = endpointConfig;
            _requestQueueService = requestQueueService;
        }

        public UniTask<HttpResponse<WeatherForecastResponse>> GetForecastAsync()
        {
            HttpRequest<WeatherForecastResponse> request = new HttpRequest<WeatherForecastResponse>(RequestScopeType.Weather, _endpointConfig.ForecastUrl);
            UniTask<HttpResponse<WeatherForecastResponse>> responseTask = _requestQueueService.Enqueue(request);

            return responseTask;
        }
    }
}
