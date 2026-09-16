using CkrSystem.Network;
using UnityEngine;
using Zenject;

namespace CkrSystem.Network.Features.Dogs
{
    [CreateAssetMenu(fileName = "Dog Network Installer", menuName = "CKR System/Dogs/Network Installer")]
    public class DogNetworkInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private DogEndpointConfig _endpointConfig;

        public override void InstallBindings()
        {
            Container.BindInstance(_endpointConfig).AsSingle();
            Container.Bind<DogApiClient>().AsSingle();
        }
    }
}
