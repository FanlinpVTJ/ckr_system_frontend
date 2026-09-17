using System;
using Cysharp.Threading.Tasks;
using Zenject;

namespace CkrSystem.Save
{
    public class SaveSystemLifecycleHandler : IInitializable, IDisposable
    {
        private readonly SaveCoordinator _saveCoordinator;

        public SaveSystemLifecycleHandler(SaveCoordinator saveCoordinator)
        {
            _saveCoordinator = saveCoordinator;
        }

        public void Initialize()
        {
            LoadAsync().Forget();
        }

        public void Dispose()
        {
            SaveAsync().Forget();
        }

        private async UniTaskVoid LoadAsync()
        {
            await _saveCoordinator.LoadAllAsync();
        }

        private async UniTaskVoid SaveAsync()
        {
            await _saveCoordinator.SaveAllAsync();
        }
    }
}
