using TMPro;
using UnityEngine;
using WindowsManager;

namespace CkrSystem.Network.UI
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
