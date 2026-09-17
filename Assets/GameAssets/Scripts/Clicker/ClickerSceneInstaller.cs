using Zenject;

namespace CkrSystem.Clicker
{
    public class ClickerSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ClickerView>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<ClickerPresenter>().AsSingle();
        }
    }
}
