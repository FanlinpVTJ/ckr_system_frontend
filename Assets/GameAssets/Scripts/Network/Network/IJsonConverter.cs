namespace FeaturedClicker.Network
{
    public interface IJsonConverter
    {
        bool TryDeserialize<TResponse>(string responseBody, out TResponse data, out string errorMessage);
    }
}
