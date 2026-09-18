using WindowsManager;

namespace CkrSystem.Network.UI
{
    public class NetworkErrorWindowService : INetworkErrorWindowService
    {
        private readonly IWindowsManager _windowsManager;
        private readonly WindowData _networkErrorWindowData;

        public NetworkErrorWindowService(IWindowsManager windowsManager, WindowData networkErrorWindowData)
        {
            _windowsManager = windowsManager;
            _networkErrorWindowData = networkErrorWindowData;
        }

        public void Show(string errorMessage)
        {
            Window window = _windowsManager.OpenWindow(_networkErrorWindowData);
            NetworkErrorWindow networkErrorWindow = window.GetComponent<NetworkErrorWindow>();
            networkErrorWindow.SetErrorText(errorMessage);
        }
    }
}
