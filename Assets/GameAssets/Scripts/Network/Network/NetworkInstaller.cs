using UnityEngine;
using Zenject;

namespace CkrSystem.Network
{
    [CreateAssetMenu(fileName = "Network Installer", menuName = "CKR System/Network/Network Installer")]
    public class NetworkInstaller : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IJsonConverter>().To<JsonConverter>().AsSingle();
            Container.Bind<IHttpRequestExecutor>().To<UnityWebRequestExecutor>().AsSingle();
            Container.Bind<RequestQueueService>().AsSingle();
        }
    }
}
