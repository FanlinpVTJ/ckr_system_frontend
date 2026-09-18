using Zenject;

namespace CkrSystem.Weather
{
    public class WeatherSceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<WeatherTabView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<WeatherForecastMapper>().AsSingle();
            Container.BindInterfacesAndSelfTo<WeatherPresenter>().AsSingle();
        }
    }
}
