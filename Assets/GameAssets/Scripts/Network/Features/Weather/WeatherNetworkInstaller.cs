using CkrSystem.Network;
using UnityEngine;
using Zenject;

namespace CkrSystem.Network.Features.Weather
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
