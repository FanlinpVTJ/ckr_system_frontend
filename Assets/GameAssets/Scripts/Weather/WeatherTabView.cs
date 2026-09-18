using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CkrSystem.Weather
{
    public class WeatherTabView : MonoBehaviour
    {
        public event Action OnShown;
        public event Action OnHidden;

        [SerializeField] private WeatherIconConfig _weatherIconConfig;
        [SerializeField] private Image _weatherIconImage;
        [SerializeField] private TMP_Text _forecastText;
        [SerializeField] private GameObject _loadingObject;
        [SerializeField] private GameObject _weatherObject;

        public bool IsVisible => isActiveAndEnabled;

        private void OnEnable()
        {
            _weatherObject.SetActive(false);
            OnShown?.Invoke();
        }

        private void OnDisable()
        {
            OnHidden?.Invoke();
        }

        public void ShowLoading()
        {
            _loadingObject.SetActive(true);
        }

        public void ShowForecast(WeatherForecastModel weatherForecast)
        {
            _weatherObject.SetActive(true);
            _weatherIconImage.sprite = _weatherIconConfig.GetIcon(weatherForecast.IconUrl);
            _forecastText.text = $"{weatherForecast.Title}\n{weatherForecast.Temperature}{weatherForecast.TemperatureUnit}";
            _loadingObject.SetActive(false);
        }

        public void HideLoading()
        {
            _loadingObject.SetActive(false);
        }
    }
}
