using UnityEngine;

namespace FeaturedClicker.Sound
{
    public class SoundManager : ISoundManager
    {
        private readonly SoundConfig _soundConfig;
        private readonly AudioSource _audioSource;

        public SoundManager(SoundConfig soundConfig, AudioSource audioSource)
        {
            _soundConfig = soundConfig;
            _audioSource = audioSource;
        }

        public bool Play(string soundId)
        {
            SoundDefinition soundDefinition = _soundConfig.GetSoundDefinition(soundId);

            if (soundDefinition == null || soundDefinition.AudioClip == null)
            {
                return false;
            }

            _audioSource.pitch = soundDefinition.Pitch;
            _audioSource.PlayOneShot(soundDefinition.AudioClip, soundDefinition.Volume);

            return true;
        }
    }
}
