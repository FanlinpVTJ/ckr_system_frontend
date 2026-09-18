using UnityEngine;

namespace FeaturedClicker.Network.Features.Weather
{
    [CreateAssetMenu(fileName = "Weather Endpoint Config", menuName = "CKR System/Weather/Endpoint Config")]
    public class WeatherEndpointConfig : ScriptableObject
    {
        [SerializeField] private string _forecastUrl = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";

        public string ForecastUrl => _forecastUrl;
    }
}
