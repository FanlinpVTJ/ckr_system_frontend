using Cysharp.Threading.Tasks;

namespace FeaturedClicker.Save
{
    public interface ISaveSystem
    {
        UniTask<T> LoadAsync<T>(string key) where T : class, new();
        UniTask SaveAsync<T>(string key, T data) where T : class;
        UniTask DeleteAsync(string key);
    }
}
