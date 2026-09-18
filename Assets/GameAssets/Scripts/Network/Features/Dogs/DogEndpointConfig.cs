using UnityEngine;

namespace FeaturedClicker.Network.Features.Dogs
{
    [CreateAssetMenu(fileName = "Dog Endpoint Config", menuName = "CKR System/Dogs/Endpoint Config")]
    public class DogEndpointConfig : ScriptableObject
    {
        [SerializeField] private string _baseUrl = "https://dogapi.dog/api/v2";

        public string BaseUrl => _baseUrl;
    }
}
