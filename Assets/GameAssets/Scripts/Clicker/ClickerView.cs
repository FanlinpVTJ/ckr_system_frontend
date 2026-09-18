using FeaturedClicker.Sound;
using Coffee.UIExtensions;
using System;
using UnityEngine;
using UnityEngine.UI;
using WindowsManager.UI;
using Zenject;

namespace FeaturedClicker.Clicker
{
    public class ClickerView : MonoBehaviour
    {
        public event Action OnCollectionButtonClicked;
        public event Action OnShown;
        public event Action OnHidden;

        [SerializeField] private Button _collectionButton;
        [SerializeField] private UIParticle _collectionUIParticle;
        [SerializeField] private ButtonPressAnimation _collectionButtonTween;
        [SerializeField] private string _soundId;

        private ISoundManager _soundManager;

        [Inject]
        private void Construct(ISoundManager soundManager)
        {
            _soundManager = soundManager;
        }
        private void Awake()
        {
            _collectionButton.onClick.AddListener(HandleCollectionButtonClick);
        }

        private void OnEnable()
        {
            OnShown?.Invoke();
        }

        private void OnDisable()
        {
            OnHidden?.Invoke();
        }

        private void OnDestroy()
        {
            _collectionButton.onClick.RemoveListener(HandleCollectionButtonClick);
        }

        public void PlayCollectionEffects()
        {
            _soundManager.Play(_soundId);
            _collectionButtonTween.PlayAnimation();
            _collectionUIParticle.Play();
        }

        private void HandleCollectionButtonClick()
        {
            OnCollectionButtonClicked?.Invoke();
        }
    }
}
