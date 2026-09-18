using System;
using UnityEngine;

namespace FeaturedClicker.Weather
{
    [Serializable]
    public class WeatherIconDefinition
    {
        [SerializeField] private string _urlIdentifier;
        [SerializeField] private Sprite _sprite;

        public string UrlIdentifier => _urlIdentifier;
        public Sprite Sprite => _sprite;
    }
}
