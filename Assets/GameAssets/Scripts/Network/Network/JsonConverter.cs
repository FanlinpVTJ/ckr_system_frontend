using Newtonsoft.Json;

namespace FeaturedClicker.Network
{
    public class JsonConverter : IJsonConverter
    {
        public bool TryDeserialize<TResponse>(string responseBody, out TResponse data, out string errorMessage)
        {
            try
            {
                data = JsonConvert.DeserializeObject<TResponse>(responseBody);
                errorMessage = string.Empty;

                return true;
            }
            catch (JsonException exception)
            {
                data = default;
                errorMessage = exception.Message;

                return false;
            }
        }
    }
}
