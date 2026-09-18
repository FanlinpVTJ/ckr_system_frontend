using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ValueSystem;

namespace CkrSystem.Save
{
    public class ValueSaveParticipant : ISaveParticipant
    {
        private const string SAVE_KEY = "values";

        private readonly IValueSystem _valueSystem;
        private readonly ISaveSystem _saveSystem;

        private bool _isSubscribed;

        public ValueSaveParticipant(IValueSystem valueSystem, ISaveSystem saveSystem)
        {
            _valueSystem = valueSystem;
            _saveSystem = saveSystem;
        }

        public async UniTask LoadAsync()
        {
            ValuesSaveData saveData = await _saveSystem.LoadAsync<ValuesSaveData>(SAVE_KEY);

            foreach (ValueSaveEntry entry in saveData.Entries)
            {
                _valueSystem.SetValue(entry.Id, entry.Amount);
            }

            SubscribeToValueChanges();
        }

        public UniTask SaveAsync()
        {
            ValuesSaveData saveData = new ValuesSaveData();

            foreach (ValueHandler valueHandler in _valueSystem.ValueHandler.Values)
            {
                ValueSaveEntry entry = new ValueSaveEntry();
                entry.Id = valueHandler.Id;
                entry.Amount = valueHandler.Value;
                saveData.Entries.Add(entry);
            }

            return _saveSystem.SaveAsync(SAVE_KEY, saveData);
        }

        private void SubscribeToValueChanges()
        {
            if (_isSubscribed)
            {
                return;
            }

            foreach (ValueHandler valueHandler in _valueSystem.ValueHandler.Values)
            {
                valueHandler.OnValueChanged += SaveValues;
            }

            _isSubscribed = true;
        }

        private void SaveValues()
        {
            SaveAsync().Forget();
        }
    }
}
