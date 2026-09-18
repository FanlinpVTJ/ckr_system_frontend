using UnityEngine;
using Cysharp.Threading.Tasks;

namespace FeaturedClicker.Save
{
    public class PlayerPrefsSaveSystem : ISaveSystem
    {
        public UniTask<T> LoadAsync<T>(string key) where T : class, new()
        {
            if (!PlayerPrefs.HasKey(key))
            {
                T defaultData = new T();

                return UniTask.FromResult(defaultData);
            }

            string serializedData = PlayerPrefs.GetString(key);
            T data = JsonUtility.FromJson<T>(serializedData);

            return UniTask.FromResult(data);
        }

        public UniTask SaveAsync<T>(string key, T data) where T : class
        {
            string serializedData = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(key, serializedData);
            PlayerPrefs.Save();

            return UniTask.CompletedTask;
        }

        public UniTask DeleteAsync(string key)
        {
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();

            return UniTask.CompletedTask;
        }
    }
}
