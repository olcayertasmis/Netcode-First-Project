using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
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
        [SerializeField] private TMP_Dropdown gameMode;
        [SerializeField] private Toggle isLobbyPrivate;
        [SerializeField] private TMP_Text joinCodeText;

        public async void CreateLobbyMethod()
        {
            string _lobbyName = this.lobbyName.text;
            int _maxPlayerCount = Convert.ToInt32(maxPlayer.options[maxPlayer.value].text);
            CreateLobbyOptions options = new CreateLobbyOptions();
            options.IsPrivate = isLobbyPrivate.isOn;

            // Player Creation
            options.Player = new Player(AuthenticationService.Instance.PlayerId);
            options.Player.Data = new Dictionary<string, PlayerDataObject>()
            {
                { "PlayerLevel", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, "5") }
            };

            // Lobby Data
            options.Data = new Dictionary<string, DataObject>()
            {
                { "GameMode", new DataObject(DataObject.VisibilityOptions.Public, gameMode.options[gameMode.value].text, DataObject.IndexOptions.S1) }
            };

            Lobby lobby = await LobbyService.Instance.CreateLobbyAsync(_lobbyName, _maxPlayerCount, options);
            GetComponent<CurrentLobby>().currentLobby = lobby;
            DontDestroyOnLoad(this);
            Debug.Log("Create Lobby Done!");

            LobbyStatic.LogPlayersInLobby(lobby);
            LobbyStatic.LogLobby(lobby);

            joinCodeText.text = lobby.LobbyCode;

            StartCoroutine(HeartbeatLobbyCorotine(lobby.Id, 15f));

            LobbyStatic.LoadLobbyRoom();
        }

        IEnumerator HeartbeatLobbyCorotine(string lobbyID, float waitTimeSeconds)
        {
            var delay = new WaitForSeconds(waitTimeSeconds);

            while (true)
            {
                LobbyService.Instance.SendHeartbeatPingAsync(lobbyID);
                yield return delay;
            }
        }
    }
}