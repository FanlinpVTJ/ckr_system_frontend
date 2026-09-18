using WindowsManager;

namespace FeaturedClicker.Dogs
{
    public class DogBreedDetailsWindowService : IDogBreedDetailsWindowService
    {
        private readonly IWindowsManager _windowsManager;
        private readonly WindowData _dogBreedDetailsWindowData;

        public DogBreedDetailsWindowService(IWindowsManager windowsManager, WindowData dogBreedDetailsWindowData)
        {
            _windowsManager = windowsManager;
            _dogBreedDetailsWindowData = dogBreedDetailsWindowData;
        }

        public void Show(DogBreedModel breed)
        {
            Window window = _windowsManager.OpenWindow(_dogBreedDetailsWindowData);
            DogBreedDetailsWindow dogBreedDetailsWindow = window.GetComponent<DogBreedDetailsWindow>();
            dogBreedDetailsWindow.SetBreed(breed);
        }

        public void Close()
        {
            Window window = _windowsManager.GetWindowByData(_dogBreedDetailsWindowData);

            if (window == null)
            {
                return;
            }

            _windowsManager.CloseWindow(_dogBreedDetailsWindowData);
        }
    }
}
