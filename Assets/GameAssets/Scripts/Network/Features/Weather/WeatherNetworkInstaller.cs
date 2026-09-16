using CkrSystem.Network;
using UnityEngine;
using Zenject;

namespace CkrSystem.Network.Features.Weather
{
    [CreateAssetMenu(fileName = "Weather Network Installer", menuName = "CKR System/Weather/Network Installer")]
    public class WeatherNetworkInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private WeatherEndpointConfig _endpointConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_endpointConfig).AsSingle();
            Container.Bind<WeatherApiClient>().AsSingle();
        }
    }
}
