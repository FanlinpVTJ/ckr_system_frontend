using UnityEngine;

namespace CkrSystem.Network
{
    [CreateAssetMenu(fileName = "Endpoint Config", menuName = "CKR System/Network/Endpoint Config")]
    public class EndpointConfig : ScriptableObject
    {
        [SerializeField] private string _weatherForecastUrl = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";
        [SerializeField] private string _dogApiBaseUrl = "https://dogapi.dog/api/v2";

        public string WeatherForecastUrl => _weatherForecastUrl;
        public string DogApiBaseUrl => _dogApiBaseUrl;
    }
}
