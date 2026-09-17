using Cysharp.Threading.Tasks;

namespace CkrSystem.Save
{
    public interface ISaveSystem
    {
        UniTask<T> LoadAsync<T>(string key) where T : class, new();
        UniTask SaveAsync<T>(string key, T data) where T : class;
        UniTask DeleteAsync(string key);
    }
}
