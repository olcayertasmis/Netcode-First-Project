using System;
using TMPro;
using Unity.Services.Lobbies;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Lobbies.Models;

namespace LobbyScripts
{
    public class CreateLobby : MonoBehaviour
    {
        [SerializeField] private TMP_InputField lobbyName;
        [SerializeField] private TMP_Dropdown maxPlayer;
        [SerializeField] private Toggle isLobbyPrivate;

        public async void CreateLobbyMethod()
        {
            string _lobbyName = this.lobbyName.text;
            int _maxPlayerCount = Convert.ToInt32(maxPlayer.options[maxPlayer.value].text);
            CreateLobbyOptions options = new CreateLobbyOptions();
            options.IsPrivate = isLobbyPrivate.isOn;

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(_lobbyName, _maxPlayerCount, options);
            DontDestroyOnLoad(this);
        }
    }
}