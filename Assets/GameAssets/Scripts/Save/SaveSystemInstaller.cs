using Zenject;

namespace FeaturedClicker.Save
{
    public class SaveSystemInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ISaveSystem>().To<PlayerPrefsSaveSystem>().AsSingle();
            Container.Bind<ISaveParticipant>().To<ValueSaveParticipant>().AsSingle();
            Container.Bind<SaveCoordinator>().AsSingle();
            Container.BindInterfacesAndSelfTo<SaveSystemLifecycleHandler>().AsSingle().NonLazy();
        }
    }
}
