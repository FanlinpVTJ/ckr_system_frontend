using System;
using FeaturedClicker.Network;
using FeaturedClicker.Network.Features.Dogs;
using FeaturedClicker.Network.UI;
using Cysharp.Threading.Tasks;
using Zenject;

namespace FeaturedClicker.Dogs
{
    public class DogBreedsPresenter : IInitializable, IDisposable
    {
        private readonly DogApiClient _dogApiClient;
        private readonly RequestQueueService _requestQueueService;
        private readonly DogBreedMapper _dogBreedMapper;
        private readonly DogBreedsTabView _dogBreedsTabView;
        private readonly IDogBreedDetailsWindowService _dogBreedDetailsWindowService;
        private readonly INetworkErrorWindowService _networkErrorWindowService;

        private int _breedsRequestVersion;
        private int _breedDetailsRequestVersion;
        private bool _isBreedsRequestPending;
        private bool _isBreedDetailsRequestPending;

        public DogBreedsPresenter(
            DogApiClient dogApiClient,
            RequestQueueService requestQueueService,
            DogBreedMapper dogBreedMapper,
            DogBreedsTabView dogBreedsTabView,
            IDogBreedDetailsWindowService dogBreedDetailsWindowService,
            INetworkErrorWindowService networkErrorWindowService)
        {
            _dogApiClient = dogApiClient;
            _requestQueueService = requestQueueService;
            _dogBreedMapper = dogBreedMapper;
            _dogBreedsTabView = dogBreedsTabView;
            _dogBreedDetailsWindowService = dogBreedDetailsWindowService;
            _networkErrorWindowService = networkErrorWindowService;
        }

        public void Initialize()
        {
            _dogBreedsTabView.OnShown += HandleTabShown;
            _dogBreedsTabView.OnHidden += HandleTabHidden;
            _dogBreedsTabView.OnBreedSelected += HandleBreedSelected;

            if (_dogBreedsTabView.IsVisible)
            {
                StartDogBreedsUpdates();
            }
        }

        public void Dispose()
        {
            _dogBreedsTabView.OnShown -= HandleTabShown;
            _dogBreedsTabView.OnHidden -= HandleTabHidden;
            _dogBreedsTabView.OnBreedSelected -= HandleBreedSelected;
            StopDogBreedsUpdates();
        }

        private void HandleTabShown()
        {
            StartDogBreedsUpdates();
        }

        private void HandleTabHidden()
        {
            StopDogBreedsUpdates();
        }

        private void HandleBreedSelected(DogBreedModel breed)
        {
            _requestQueueService.CancelRequests(RequestScopeType.DogBreedDetails);
            _breedDetailsRequestVersion++;
            _isBreedDetailsRequestPending = false;
            _dogBreedsTabView.HideBreedDetailsLoading();
            RequestBreedDetails(breed.Id);
        }

        private void StartDogBreedsUpdates()
        {
            RequestBreeds();
        }

        private void StopDogBreedsUpdates()
        {
            _isBreedsRequestPending = false;
            _isBreedDetailsRequestPending = false;
            _breedsRequestVersion++;
            _breedDetailsRequestVersion++;
            _requestQueueService.CancelRequests(RequestScopeType.DogBreeds);
            _requestQueueService.CancelRequests(RequestScopeType.DogBreedDetails);
            _dogBreedsTabView.HideBreedsLoading();
            _dogBreedsTabView.HideBreedDetailsLoading();
            _dogBreedDetailsWindowService.Close();
        }

        private void RequestBreeds()
        {
            if (_isBreedsRequestPending)
            {
                return;
            }

            _isBreedsRequestPending = true;
            _dogBreedsTabView.ShowBreedsLoading();
            LoadBreedsAsync(_breedsRequestVersion).Forget();
        }

        private void RequestBreedDetails(string breedId)
        {
            _isBreedDetailsRequestPending = true;
            _dogBreedsTabView.ShowBreedDetailsLoading();
            LoadBreedDetailsAsync(breedId, _breedDetailsRequestVersion).Forget();
        }

        private async UniTaskVoid LoadBreedsAsync(int requestVersion)
        {
            HttpResponse<DogBreedsResponse> response = await _dogApiClient.GetBreedsAsync();

            if (requestVersion != _breedsRequestVersion)
            {
                return;
            }

            _isBreedsRequestPending = false;

            if (response.IsCancelled)
            {
                return;
            }

            if (!response.IsSuccess)
            {
                ShowNetworkError(response.ErrorMessage);

                return;
            }

            DogBreedModel[] breeds;

            if (!_dogBreedMapper.TryMapBreeds(response.Data, out breeds))
            {
                ShowNetworkError("Dog breeds response is invalid.");

                return;
            }

            _dogBreedsTabView.ShowBreeds(breeds);
        }

        private async UniTaskVoid LoadBreedDetailsAsync(string breedId, int requestVersion)
        {
            HttpResponse<DogBreedResponse> response = await _dogApiClient.GetBreedAsync(breedId);

            if (requestVersion != _breedDetailsRequestVersion)
            {
                return;
            }

            _isBreedDetailsRequestPending = false;
            _dogBreedsTabView.HideBreedDetailsLoading();

            if (response.IsCancelled)
            {
                return;
            }

            if (!response.IsSuccess)
            {
                ShowNetworkError(response.ErrorMessage);

                return;
            }

            DogBreedModel breed;

            if (!_dogBreedMapper.TryMapBreed(response.Data, out breed))
            {
                ShowNetworkError("Dog breed response is invalid.");

                return;
            }

            _dogBreedDetailsWindowService.Show(breed);
        }

        private void ShowNetworkError(string errorMessage)
        {
            _dogBreedsTabView.HideBreedsLoading();
            _dogBreedsTabView.HideBreedDetailsLoading();
            _networkErrorWindowService.Show(errorMessage);
        }
    }
}
