using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Relay
{
    public class RelayManager : MonoBehaviour
    {
        private string _playerId;

        [SerializeField] private TextMeshProUGUI idText;

        private async void Start()
        {
            await UnityServices.InitializeAsync();
            Debug.Log("Unity Service Init");
            SignIn();
        }

        private async void SignIn()
        {
            Debug.Log("Sign in");
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            _playerId = AuthenticationService.Instance.PlayerId;
            Debug.Log("Signed in : " + _playerId);
            idText.text = _playerId;
        }

        void Update()
        {
        }
    }
}