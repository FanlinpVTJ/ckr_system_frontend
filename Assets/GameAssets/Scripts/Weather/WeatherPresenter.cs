using System;
using FeaturedClicker.Network;
using FeaturedClicker.Network.Features.Weather;
using FeaturedClicker.Network.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace FeaturedClicker.Weather
{
    public class WeatherPresenter : IInitializable, ITickable, IDisposable
    {
        private const float REFRESH_INTERVAL = 5f;

        private readonly WeatherApiClient _weatherApiClient;
        private readonly RequestQueueService _requestQueueService;
        private readonly WeatherForecastMapper _weatherForecastMapper;
        private readonly WeatherTabView _weatherTabView;
        private readonly INetworkErrorWindowService _networkErrorWindowService;

        private float _refreshElapsedTime;
        private int _requestVersion;
        private bool _isWeatherTabVisible;
        private bool _isRequestPending;

        public WeatherPresenter(
            WeatherApiClient weatherApiClient,
            RequestQueueService requestQueueService,
            WeatherForecastMapper weatherForecastMapper,
            WeatherTabView weatherTabView,
            INetworkErrorWindowService networkErrorWindowService)
        {
            _weatherApiClient = weatherApiClient;
            _requestQueueService = requestQueueService;
            _weatherForecastMapper = weatherForecastMapper;
            _weatherTabView = weatherTabView;
            _networkErrorWindowService = networkErrorWindowService;
        }

        public void Initialize()
        {
            _weatherTabView.OnShown += HandleTabShown;
            _weatherTabView.OnHidden += HandleTabHidden;

            if (_weatherTabView.IsVisible)
            {
                StartWeatherUpdates();
            }
        }

        public void Tick()
        {
            if (!_isWeatherTabVisible || _isRequestPending)
            {
                return;
            }

            _refreshElapsedTime += Time.deltaTime;

            if (_refreshElapsedTime < REFRESH_INTERVAL)
            {
                return;
            }

            _refreshElapsedTime = 0;
            RequestForecast();
        }

        public void Dispose()
        {
            _weatherTabView.OnShown -= HandleTabShown;
            _weatherTabView.OnHidden -= HandleTabHidden;
            StopWeatherUpdates();
        }

        private void HandleTabShown()
        {
            StartWeatherUpdates();
        }

        private void HandleTabHidden()
        {
            StopWeatherUpdates();
        }

        private void StartWeatherUpdates()
        {
            _isWeatherTabVisible = true;
            _refreshElapsedTime = 0;
            RequestForecast();
        }

        private void StopWeatherUpdates()
        {
            _isWeatherTabVisible = false;
            _isRequestPending = false;
            _refreshElapsedTime = 0;
            _requestVersion++;
            _requestQueueService.CancelRequests(RequestScopeType.Weather);
        }

        private void RequestForecast()
        {
            if (_isRequestPending)
            {
                return;
            }

            _isRequestPending = true;
            _weatherTabView.ShowLoading();
            LoadForecastAsync(_requestVersion).Forget();
        }

        private async UniTaskVoid LoadForecastAsync(int requestVersion)
        {
            HttpResponse<WeatherForecastResponse> response = await _weatherApiClient.GetForecastAsync();

            if (requestVersion != _requestVersion)
            {
                return;
            }

            _isRequestPending = false;

            if (response.IsCancelled)
            {
                return;
            }

            if (!response.IsSuccess)
            {
                ShowNetworkError(response.ErrorMessage);

                return;
            }

            WeatherForecastModel weatherForecast;

            if (!_weatherForecastMapper.TryMap(response.Data, out weatherForecast))
            {
                ShowNetworkError("Weather response is invalid.");

                return;
            }

            _weatherTabView.ShowForecast(weatherForecast);
        }

        private void ShowNetworkError(string errorMessage)
        {
            _weatherTabView.HideLoading();
            _networkErrorWindowService.Show(errorMessage);
        }
    }
}
