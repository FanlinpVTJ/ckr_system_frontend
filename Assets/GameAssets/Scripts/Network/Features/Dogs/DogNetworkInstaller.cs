using FeaturedClicker.Network;
using UnityEngine;
using Zenject;

namespace FeaturedClicker.Network.Features.Dogs
{
    public class DogNetworkInstaller : MonoInstaller
    {
        [SerializeField] private DogEndpointConfig _endpointConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_endpointConfig).AsSingle();
            Container.Bind<DogApiClient>().AsSingle();
        }
    }
}
