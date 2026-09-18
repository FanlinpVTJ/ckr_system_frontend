using TMPro;
using UnityEngine;
using WindowsManager;

namespace FeaturedClicker.Network.UI
{
    public class NetworkErrorWindow : Window
    {
        [SerializeField] private TMP_Text _errorText;

        public void SetErrorText(string errorMessage)
        {
            _errorText.text = errorMessage;
        }
    }
}
