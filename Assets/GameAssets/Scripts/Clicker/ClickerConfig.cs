using UnityEngine;
using ValueSystem;

namespace FeaturedClicker.Clicker
{
    [CreateAssetMenu(fileName = "Clicker Config", menuName = "CKR System/Clicker/Config")]
    public class ClickerConfig : ScriptableObject
    {
        [SerializeField] private ValueData _currencyValueData;
        [SerializeField] private ValueData _energyValueData;
        [SerializeField] private float _collectionReward = 1;
        [SerializeField] private float _collectionEnergyCost = 1;
        [SerializeField] private float _automaticCollectionInterval = 3;
        [SerializeField] private float _energyRecoveryAmount = 10;
        [SerializeField] private float _energyRecoveryInterval = 10;

        public ValueData CurrencyValueData => _currencyValueData;
        public ValueData EnergyValueData => _energyValueData;
        public float CollectionReward => _collectionReward;
        public float CollectionEnergyCost => _collectionEnergyCost;
        public float AutomaticCollectionInterval => _automaticCollectionInterval;
        public float EnergyRecoveryAmount => _energyRecoveryAmount;
        public float EnergyRecoveryInterval => _energyRecoveryInterval;
    }
}
