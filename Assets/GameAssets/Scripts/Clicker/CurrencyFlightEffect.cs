using PoolsUtility;
using TweenComponents.Base;
using UnityEngine;
using Zenject;

namespace CkrSystem.Clicker
{
    public class CurrencyFlightEffect : MonoBehaviour
    {
        [SerializeField] private CurrencyFlightIcon _currencyIconPrefab;
        [SerializeField] private string _poolId = "CurrencyFlightIcon";
        [SerializeField] [Min(1)] private int _initialPoolSize = 5;
        [SerializeField] [Min(1)] private int _maxPoolSize = 20;
        [SerializeField] private PoolExpandMethods _poolExpandMethod = PoolExpandMethods.Double;
        [SerializeField] private RectTransform _sourceRectTransform;
        [SerializeField] private RectTransform _targetRectTransform;
        [SerializeField] private float _flightDuration = 0.5f;
        [SerializeField] private TweenBase _targetTween;

        private PoolManager _poolManager;
        private PoolGroup _currencyIconPoolGroup;

        private void Awake()
        {
            _currencyIconPoolGroup = new PoolGroup(
                _poolId,
                _currencyIconPrefab,
                _initialPoolSize,
                _maxPoolSize,
                _poolExpandMethod);
        }

        [Inject]
        private void Construct(PoolManager poolManager)
        {
            _poolManager = poolManager;
        }

        public void Play(Sprite currencyIcon)
        {
            PooledObject pooledObject = _poolManager.InstantiateFromGroup(_currencyIconPoolGroup, transform);
            CurrencyFlightIcon currencyFlightIcon = pooledObject.GetComponent<CurrencyFlightIcon>();
            currencyFlightIcon.Play(
                currencyIcon,
                _sourceRectTransform.position,
                _targetRectTransform.position,
                _flightDuration,
                _targetTween);
        }
    }
}
