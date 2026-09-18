using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WindowsManager;

namespace FeaturedClicker.Dogs
{
    public class DogBreedDetailsWindow : Window
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private RectTransform _contentRectTransform;

        public void SetBreed(DogBreedModel breed)
        {
            _nameText.text = breed.Name;
            _descriptionText.text = breed.Description;
            LayoutRebuilder.ForceRebuildLayoutImmediate(_contentRectTransform);
        }
    }
}
