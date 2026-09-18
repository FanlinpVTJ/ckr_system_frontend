using System;
using ValueSystem;
using Zenject;

namespace FeaturedClicker.Clicker
{
    public class ClickerService : IInitializable, ITickable
    {
        public event Action OnCollected;

        private readonly ClickerConfig _clickerConfig;
        private readonly IValueSystem _valueSystem;

        private float _automaticCollectionElapsedTime;
        private float _energyRecoveryElapsedTime;
        private bool _isAutomaticCollectionEnabled;

        public ClickerService(ClickerConfig clickerConfig, IValueSystem valueSystem)
        {
            _clickerConfig = clickerConfig;
            _valueSystem = valueSystem;
        }

        public void Initialize()
        {
            _automaticCollectionElapsedTime = 0;
            _energyRecoveryElapsedTime = 0;
        }

        public void Tick()
        {
            ProcessAutomaticCollection();
            ProcessEnergyRecovery();
        }

        public bool TryCollectManually()
        {
            _automaticCollectionElapsedTime = 0;

            return TryCollect();
        }

        public void SetAutomaticCollectionEnabled(bool isEnabled)
        {
            _isAutomaticCollectionEnabled = isEnabled;
            _automaticCollectionElapsedTime = 0;
        }

        private bool TryCollect()
        {
            if (!_valueSystem.TrySubtract(_clickerConfig.EnergyValueData, _clickerConfig.CollectionEnergyCost))
            {
                return false;
            }

            _valueSystem.Add(_clickerConfig.CurrencyValueData, _clickerConfig.CollectionReward);
            OnCollected?.Invoke();

            return true;
        }

        private void ProcessAutomaticCollection()
        {
            if (!_isAutomaticCollectionEnabled)
            {
                return;
            }

            _automaticCollectionElapsedTime += UnityEngine.Time.deltaTime;

            if (_automaticCollectionElapsedTime < _clickerConfig.AutomaticCollectionInterval)
            {
                return;
            }

            _automaticCollectionElapsedTime -= _clickerConfig.AutomaticCollectionInterval;
            TryCollect();
        }

        private void ProcessEnergyRecovery()
        {
            _energyRecoveryElapsedTime += UnityEngine.Time.deltaTime;

            if (_energyRecoveryElapsedTime < _clickerConfig.EnergyRecoveryInterval)
            {
                return;
            }

            _energyRecoveryElapsedTime -= _clickerConfig.EnergyRecoveryInterval;
            _valueSystem.Add(_clickerConfig.EnergyValueData, _clickerConfig.EnergyRecoveryAmount);
        }
    }
}
