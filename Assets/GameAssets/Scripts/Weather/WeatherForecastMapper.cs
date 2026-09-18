using CkrSystem.Network.Features.Weather;

namespace CkrSystem.Weather
{
    public class WeatherForecastMapper
    {
        public bool TryMap(WeatherForecastResponse response, out WeatherForecastModel weatherForecast)
        {
            weatherForecast = null;

            if (response == null || response.Properties == null || response.Properties.Periods == null || response.Properties.Periods.Length == 0)
            {
                return false;
            }

            WeatherForecastPeriod currentPeriod = response.Properties.Periods[0];
            weatherForecast = new WeatherForecastModel(
                "Сегодня",
                currentPeriod.Temperature,
                currentPeriod.TemperatureUnit,
                currentPeriod.Icon);

            return true;
        }
    }
}
