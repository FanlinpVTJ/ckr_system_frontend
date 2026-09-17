using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace CkrSystem.Save
{
    public class SaveCoordinator
    {
        private readonly List<ISaveParticipant> _saveParticipants;

        public SaveCoordinator(List<ISaveParticipant> saveParticipants)
        {
            _saveParticipants = saveParticipants;
        }

        public async UniTask LoadAllAsync()
        {
            foreach (ISaveParticipant saveParticipant in _saveParticipants)
            {
                await saveParticipant.LoadAsync();
            }
        }

        public async UniTask SaveAllAsync()
        {
            foreach (ISaveParticipant saveParticipant in _saveParticipants)
            {
                await saveParticipant.SaveAsync();
            }
        }
    }
}
