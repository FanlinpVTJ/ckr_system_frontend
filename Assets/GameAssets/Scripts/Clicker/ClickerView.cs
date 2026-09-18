using System;
using Coffee.UIExtensions;
using TMPro;
using TweenComponents.Base;
using UnityEngine;
using UnityEngine.UI;

namespace CkrSystem.Clicker
{
    public class ClickerView : MonoBehaviour
    {
        public event Action OnCollectionButtonClicked;

        [SerializeField] private Button _collectionButton;
        [SerializeField] private UIParticle _collectionUIParticle;
        [SerializeField] private TweenBase _collectionButtonTween;

        private void Awake()
        {
            _collectionButton.onClick.AddListener(HandleCollectionButtonClick);
        }

        private void OnDestroy()
        {
            _collectionButton.onClick.RemoveListener(HandleCollectionButtonClick);
        }

        public void PlayCollectionEffects()
        {
            _collectionButtonTween.Execute();
            _collectionUIParticle.Play();
        }

        private void HandleCollectionButtonClick()
        {
            OnCollectionButtonClicked?.Invoke();
        }
    }
}
