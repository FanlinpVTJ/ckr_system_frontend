namespace CkrSystem.Weather
{
    public class WeatherForecastModel
    {
        public string Title { get; }
        public int Temperature { get; }
        public string TemperatureUnit { get; }
        public string IconUrl { get; }

        public WeatherForecastModel(string title, int temperature, string temperatureUnit, string iconUrl)
        {
            Title = title;
            Temperature = temperature;
            TemperatureUnit = temperatureUnit;
            IconUrl = iconUrl;
        }
    }
}
