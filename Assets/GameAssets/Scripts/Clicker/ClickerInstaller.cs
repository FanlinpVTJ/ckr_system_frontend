using UnityEngine;
using Zenject;

namespace FeaturedClicker.Clicker
{
    public class ClickerInstaller : MonoInstaller
    {
        [SerializeField] private ClickerConfig _clickerConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_clickerConfig).AsSingle();
            Container.BindInterfacesAndSelfTo<ClickerService>().AsSingle();
        }
    }
}
