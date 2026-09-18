using System;
using UnityEngine;
using Zenject;

namespace FeaturedClicker.Dogs
{
    public class DogBreedsTabView : MonoBehaviour
    {
        public event Action OnShown;
        public event Action OnHidden;
        public event Action<DogBreedModel> OnBreedSelected;

        [SerializeField] private DogBreedScrollView _dogBreedScrollView;
        [SerializeField] private GameObject _breedsLoadingObject;
        [SerializeField] private GameObject _breedDetailsLoadingObject;

        private DogBreedListDataProvider _dogBreedListDataProvider;

        public bool IsVisible => isActiveAndEnabled;

        [Inject]
        private void Construct(DogBreedListDataProvider dogBreedListDataProvider)
        {
            _dogBreedListDataProvider = dogBreedListDataProvider;
        }

        private void Awake()
        {
            _dogBreedListDataProvider.OnBreedSelected += HandleBreedSelected;
        }

        private void OnEnable()
        {
            OnShown?.Invoke();
        }

        private void OnDisable()
        {
            OnHidden?.Invoke();
        }

        private void OnDestroy()
        {
            _dogBreedListDataProvider.OnBreedSelected -= HandleBreedSelected;
        }

        public void ShowBreedsLoading()
        {
            _breedsLoadingObject.SetActive(true);
        }

        public void HideBreedsLoading()
        {
            _breedsLoadingObject.SetActive(false);
        }

        public void ShowBreeds(DogBreedModel[] breeds)
        {
            _dogBreedListDataProvider.SetBreeds(breeds);
            _dogBreedScrollView.SetBreeds(breeds);
            _breedsLoadingObject.SetActive(false);
        }

        public void ShowBreedDetailsLoading()
        {
            _breedDetailsLoadingObject.SetActive(true);
        }

        public void HideBreedDetailsLoading()
        {
            _breedDetailsLoadingObject.SetActive(false);
        }

        private void HandleBreedSelected(DogBreedModel breed)
        {
            OnBreedSelected?.Invoke(breed);
        }
    }
}
