using TMPro;
using SmartScroll;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CkrSystem.Dogs
{
    public class DogBreedListItem : SmartScrollElement
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _nameText;

        private DogBreedListDataProvider _dogBreedListDataProvider;

        [Inject]
        private void Construct(DogBreedListDataProvider dogBreedListDataProvider)
        {
            _dogBreedListDataProvider = dogBreedListDataProvider;
        }

        private void Awake()
        {
            OnDataUpdated += UpdateContent;
            _button.onClick.AddListener(HandleButtonClicked);
        }

        private void OnDestroy()
        {
            OnDataUpdated -= UpdateContent;
            _button.onClick.RemoveListener(HandleButtonClicked);
        }

        private void UpdateContent()
        {
            DogBreedModel breed = _dogBreedListDataProvider.GetBreed(Data.Index);
            _nameText.text = $"{Data.Index + 1} - {breed.Name}";
        }

        private void HandleButtonClicked()
        {
            _dogBreedListDataProvider.SelectBreed(Data.Index);
        }
    }
}
