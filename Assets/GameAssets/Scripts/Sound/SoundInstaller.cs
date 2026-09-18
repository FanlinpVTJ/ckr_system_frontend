using UnityEngine;
using Zenject;

namespace CkrSystem.Sound
{
    public class SoundInstaller : MonoInstaller
    {
        [SerializeField] private SoundConfig _soundConfig;
        [SerializeField] private AudioSource _audioSource;

        public override void InstallBindings()
        {
            Container.Bind<ISoundManager>().To<SoundManager>().AsSingle().WithArguments(_soundConfig, _audioSource);
        }
    }
}
