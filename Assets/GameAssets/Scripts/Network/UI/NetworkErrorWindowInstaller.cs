using UnityEngine;
using WindowsManager;
using Zenject;

namespace FeaturedClicker.Network.UI
{
    public class NetworkErrorWindowInstaller : MonoInstaller
    {
        [SerializeField] private WindowData _networkErrorWindowData;

        public override void InstallBindings()
        {
            Container.Bind<INetworkErrorWindowService>()
                .To<NetworkErrorWindowService>()
                .AsSingle()
                .WithArguments(_networkErrorWindowData);
        }
    }
}
