using FeaturedClicker.Network;
using UnityEngine;
using Zenject;

namespace FeaturedClicker.Network.Features.Weather
{
    public class WeatherNetworkInstaller : MonoInstaller
    {
        [SerializeField] private WeatherEndpointConfig _endpointConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_endpointConfig).AsSingle();
            Container.Bind<WeatherApiClient>().AsSingle();
        }
    }
}
