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
            _clickerView.OnShown += HandleViewShown;
            _clickerView.OnHidden += HandleViewHidden;
            _clickerService.OnCollected += HandleCollected;
            _clickerService.SetAutomaticCollectionEnabled(_clickerView.isActiveAndEnabled);
        }

        public void Dispose()
        {
            _clickerView.OnCollectionButtonClicked -= HandleCollectionButtonClick;
            _clickerView.OnShown -= HandleViewShown;
            _clickerView.OnHidden -= HandleViewHidden;
            _clickerService.OnCollected -= HandleCollected;
        }

        private void HandleCollectionButtonClick()
        {
            if (!_clickerView.isActiveAndEnabled)
            {
                return;
            }

            _clickerService.TryCollectManually();
        }

        private void HandleViewShown()
        {
            _clickerService.SetAutomaticCollectionEnabled(true);
        }

        private void HandleViewHidden()
        {
            _clickerService.SetAutomaticCollectionEnabled(false);
        }

        private void HandleCollected()
        {
            _clickerView.PlayCollectionEffects();
            _currencyFlightEffect.Play(_clickerConfig.CurrencyValueData.Icon);
        }
    }
}
