using UnityEngine;
using WindowsManager.UI;
using Zenject;

namespace CkrSystem.Sound
{
    public class ButtonSound : AbstractButton
    {
        [SerializeField] private string _soundId;

        private ISoundManager _soundManager;

        [Inject]
        private void Construct(ISoundManager soundManager)
        {
            _soundManager = soundManager;
        }

        public override void OnButtonClick()
        {
            _soundManager.Play(_soundId);
        }
    }
}
