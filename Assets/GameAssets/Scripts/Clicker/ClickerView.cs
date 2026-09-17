using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CkrSystem.Clicker
{
    public class ClickerView : MonoBehaviour
    {
        public event Action OnCollectionButtonClicked;

        [SerializeField] private Button _collectionButton;
        [SerializeField] private TMP_Text _currencyText;
        [SerializeField] private TMP_Text _energyText;

        private void Awake()
        {
            _collectionButton.onClick.AddListener(HandleCollectionButtonClick);
        }

        private void OnDestroy()
        {
            _collectionButton.onClick.RemoveListener(HandleCollectionButtonClick);
        }

        public void SetCurrency(float currency)
        {
            _currencyText.text = currency.ToString();
        }

        public void SetEnergy(float energy)
        {
            _energyText.text = energy.ToString();
        }

        public void PlayCollectionEffects()
        {
        }

        private void HandleCollectionButtonClick()
        {
            OnCollectionButtonClicked?.Invoke();
        }
    }
}
