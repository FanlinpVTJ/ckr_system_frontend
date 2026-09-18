using UnityEngine;

namespace FeaturedClicker.Sound
{
    [System.Serializable]
    public class SoundDefinition
    {
        [SerializeField] private string _id;
        [SerializeField] private AudioClip _audioClip;
        [SerializeField] [Range(0, 1)] private float _volume = 1;
        [SerializeField] [Range(-3, 3)] private float _pitch = 1;

        public string Id => _id;
        public AudioClip AudioClip => _audioClip;
        public float Volume => _volume;
        public float Pitch => _pitch;
    }
}
