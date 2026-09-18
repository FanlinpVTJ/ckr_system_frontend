namespace FeaturedClicker.Network
{
    public class HttpHeader
    {
        public string Name { get; }
        public string Value { get; }

        public HttpHeader(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
