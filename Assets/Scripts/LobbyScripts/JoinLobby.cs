using System;
using TMPro;
using Unity.Services.Lobbies;
using UnityEngine;
using UnityEngine.UI;

namespace LobbyScripts
{
    public class JoinLobby : MonoBehaviour
    {
        [SerializeField] private TMP_InputField joinLobbyInput;

        public async void JoinLobbyWithLobbyCode(string lobbyCode)
        {
            var code = joinLobbyInput.text;
            try
            {
                await LobbyService.Instance.JoinLobbyByCodeAsync(code);
                Debug.Log("Joined Lobby With Code : " + code);
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError(e);
                throw;
            }
        }
    }
}