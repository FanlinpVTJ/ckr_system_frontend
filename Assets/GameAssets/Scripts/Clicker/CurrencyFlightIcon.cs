using FeaturedClicker.Sound;
using DG.Tweening;
using PoolsUtility;
using TweenComponents.Base;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FeaturedClicker.Clicker
{
    public class CurrencyFlightIcon : PooledObject
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private string _soundId;

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
                    _soundManager.Play(_soundId);
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
