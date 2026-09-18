using Zenject;

namespace FeaturedClicker.Dogs
{
    public class DogBreedsSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<DogBreedListDataProvider>().AsSingle();
            Container.Bind<DogBreedsTabView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<DogBreedMapper>().AsSingle();
            Container.BindInterfacesAndSelfTo<DogBreedsPresenter>().AsSingle();
        }
    }
}
