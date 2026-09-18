using CkrSystem.Sound;
using DG.Tweening;
using PoolsUtility;
using TweenComponents.Base;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CkrSystem.Clicker
{
    public class CurrencyFlightIcon : PooledObject
    {
        [SerializeField] private Image _iconImage;

        private RectTransform _rectTransform;

        private ISoundManager _soundManager;

        [Inject]
        private void Construct(ISoundManager soundManager)
        {
            _soundManager = soundManager;
        }

        private void Awake()
        {
            _rectTransform = (RectTransform)transform;
        }

        public void Play(Sprite icon, Vector3 sourcePosition, Vector3 targetPosition, float duration, TweenBase animation)
        {
            _rectTransform.DOKill();
            _iconImage.sprite = icon;
            _rectTransform.position = sourcePosition;
            _rectTransform.localScale = Vector3.one;

            _rectTransform
                .DOMove(targetPosition, duration)
                .SetEase(Ease.InQuad)
                .OnComplete(() =>
                {
                    Despawn();
                    if (animation != null)
                    {
                        animation.Execute();
                    }
                    _soundManager.Play("coins_collect");
                }
                );

        }

        public override void Despawn()
        {
            _rectTransform.DOKill();
            base.Despawn();
        }
    }
}
