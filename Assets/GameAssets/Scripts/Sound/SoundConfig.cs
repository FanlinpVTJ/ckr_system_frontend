using System.Collections.Generic;
using UnityEngine;

namespace CkrSystem.Sound
{
    [CreateAssetMenu(fileName = "Sound Config", menuName = "CKR System/Sound/Config")]
    public class SoundConfig : ScriptableObject
    {
        [SerializeField] private List<SoundDefinition> _soundDefinitions = new List<SoundDefinition>();

        public SoundDefinition GetSoundDefinition(string soundId)
        {
            foreach (SoundDefinition soundDefinition in _soundDefinitions)
            {
                if (soundDefinition.Id == soundId)
                {
                    return soundDefinition;
                }
            }

            return null;
        }
    }
}
