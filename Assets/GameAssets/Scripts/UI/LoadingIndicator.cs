using DG.Tweening;
using UnityEngine;

namespace CkrSystem.UI
{
    public class LoadingIndicator : MonoBehaviour
    {
        [SerializeField] private float _rotationDuration = 0.75f;

        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = (RectTransform)transform;
        }

        private void OnEnable()
        {
            _rectTransform.DOKill();
            _rectTransform.localRotation = Quaternion.identity;
            _rectTransform
                .DORotate(new Vector3(0, 0, -360), _rotationDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart)
                .SetUpdate(true);
        }

        private void OnDisable()
        {
            _rectTransform.DOKill();
            _rectTransform.localRotation = Quaternion.identity;
        }
    }
}
