using System;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
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
                JoinLobbyByCodeOptions options = new JoinLobbyByCodeOptions();
                options.Player = new Player(AuthenticationService.Instance.PlayerId);
                options.Player.Data = new Dictionary<string, PlayerDataObject>()
                {
                    { "PlayerLevel", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, "8") }
                };

                Lobby lobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code, options);
                Debug.Log("Joined Lobby With Code : " + code);

                LobbyStatic.LogPlayersInLobby(lobby);
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
                Debug.LogWarning("Lobby Code : " + lobby.LobbyCode);
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
                Debug.LogWarning("Lobby Code : " + lobby.LobbyCode);
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError(e);
            }
        }
    }
}