using UnityEngine;

namespace CkrSystem.Weather
{
    [CreateAssetMenu(fileName = "Weather Icon Config", menuName = "CKR System/Weather/Icon Config")]
    public class WeatherIconConfig : ScriptableObject
    {
        [SerializeField] private WeatherIconDefinition[] _iconDefinitions = new WeatherIconDefinition[0];
        [SerializeField] private Sprite _fallbackSprite;

        public Sprite GetIcon(string iconUrl)
        {
            for (int i = 0; i < _iconDefinitions.Length; i++)
            {
                WeatherIconDefinition iconDefinition = _iconDefinitions[i];

                if (iconUrl.Contains(iconDefinition.UrlIdentifier))
                {
                    return iconDefinition.Sprite;
                }
            }

            return _fallbackSprite;
        }
    }
}
