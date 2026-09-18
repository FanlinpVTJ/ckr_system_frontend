using System;
using Zenject;

namespace CkrSystem.Clicker
{
    public class ClickerPresenter : IInitializable, IDisposable
    {
        private readonly ClickerService _clickerService;
        private readonly ClickerView _clickerView;
        private readonly CurrencyFlightEffect _currencyFlightEffect;
        private readonly ClickerConfig _clickerConfig;

        public ClickerPresenter(
            ClickerService clickerService,
            ClickerView clickerView,
            CurrencyFlightEffect currencyFlightEffect,
            ClickerConfig clickerConfig)
        {
            _clickerService = clickerService;
            _clickerView = clickerView;
            _currencyFlightEffect = currencyFlightEffect;
            _clickerConfig = clickerConfig;
        }

        public void Initialize()
        {
            _clickerView.OnCollectionButtonClicked += HandleCollectionButtonClick;
            _clickerService.OnCollected += HandleCollected;
        }

        public void Dispose()
        {
            _clickerView.OnCollectionButtonClicked -= HandleCollectionButtonClick;
            _clickerService.OnCollected -= HandleCollected;
        }

        private void HandleCollectionButtonClick()
        {
            _clickerService.TryCollectManually();
        }

        private void HandleCollected()
        {
            _clickerView.PlayCollectionEffects();
            _currencyFlightEffect.Play(_clickerConfig.CurrencyValueData.Icon);
        }
    }
}
