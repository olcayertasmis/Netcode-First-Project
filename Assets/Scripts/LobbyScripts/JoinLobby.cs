using System;
using TMPro;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
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
            }
        }

        public async void JoinLobbyWithLobbyID(string lobbyID)
        {
            try
            {
                Lobby lobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyID);
                Debug.Log("Joined Lobby With ID : " + lobbyID);
                Debug.LogWarning("Lobby Code : " + lobby.LobbyCode );
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError(e);
            }
        }

        public async void QuickJoinLobby()
        {
            try
            {
                Lobby lobby = await LobbyService.Instance.QuickJoinLobbyAsync();
                Debug.Log("Joined Lobby With Quick Join : " + lobby.Id);
                Debug.LogWarning("Lobby Code : " + lobby.LobbyCode );
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError(e);
            }
        }
    }
}