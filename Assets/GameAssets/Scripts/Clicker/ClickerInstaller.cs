using UnityEngine;
using Zenject;

namespace CkrSystem.Clicker
{
    public class ClickerInstaller : MonoInstaller
    {
        [SerializeField] private ClickerConfig _clickerConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_clickerConfig).AsSingle();
            Container.Bind<ClickerView>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<ClickerService>().AsSingle();
            Container.BindInterfacesAndSelfTo<ClickerPresenter>().AsSingle();
        }
    }
}
