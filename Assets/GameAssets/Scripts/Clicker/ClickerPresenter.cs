using System;
using ValueSystem;
using Zenject;

namespace CkrSystem.Clicker
{
    public class ClickerPresenter : IInitializable, IDisposable
    {
        private readonly ClickerConfig _clickerConfig;
        private readonly ClickerService _clickerService;
        private readonly ClickerView _clickerView;
        private readonly IValueSystem _valueSystem;

        private ValueHandler _currencyValueHandler;
        private ValueHandler _energyValueHandler;

        public ClickerPresenter(
            ClickerConfig clickerConfig,
            ClickerService clickerService,
            ClickerView clickerView,
            IValueSystem valueSystem)
        {
            _clickerConfig = clickerConfig;
            _clickerService = clickerService;
            _clickerView = clickerView;
            _valueSystem = valueSystem;
        }

        public void Initialize()
        {
            _currencyValueHandler = _valueSystem.GetValueHandler(_clickerConfig.CurrencyValueData);
            _energyValueHandler = _valueSystem.GetValueHandler(_clickerConfig.EnergyValueData);
            _clickerView.OnCollectionButtonClicked += HandleCollectionButtonClick;
            _clickerService.OnCollected += HandleCollected;
            _currencyValueHandler.OnValueChanged += UpdateCurrency;
            _energyValueHandler.OnValueChanged += UpdateEnergy;
            UpdateCurrency();
            UpdateEnergy();
        }

        public void Dispose()
        {
            _clickerView.OnCollectionButtonClicked -= HandleCollectionButtonClick;
            _clickerService.OnCollected -= HandleCollected;
            _currencyValueHandler.OnValueChanged -= UpdateCurrency;
            _energyValueHandler.OnValueChanged -= UpdateEnergy;
        }

        private void HandleCollectionButtonClick()
        {
            _clickerService.TryCollect();
        }

        private void HandleCollected()
        {
            _clickerView.PlayCollectionEffects();
        }

        private void UpdateCurrency()
        {
            _clickerView.SetCurrency(_currencyValueHandler.Value);
        }

        private void UpdateEnergy()
        {
            _clickerView.SetEnergy(_energyValueHandler.Value);
        }
    }
}
