using System;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace LobbyScripts
{
    public class PopulateUI : MonoBehaviour
    {
        [Header("Other Compenents")]
        private CurrentLobby _currentLobby;

        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI lobbyName;
        [SerializeField] private TextMeshProUGUI lobbyCode;
        [SerializeField] private TextMeshProUGUI gameMode;
        [SerializeField] private TMP_InputField newLobbyName;
        [SerializeField] private TMP_InputField newPlayerName;

        [Header("Player Info")]
        [SerializeField] private GameObject playerInfoPanel;
        [SerializeField] private GameObject playerInfoPrefab;

        private string _lobbyId;

        private void Start()
        {
            _currentLobby = GameObject.Find("Lobby Manager").GetComponent<CurrentLobby>();

            if (_currentLobby.currentLobby.HostId == AuthenticationService.Instance.PlayerId) newLobbyName.gameObject.SetActive(true);
            else newLobbyName.gameObject.SetActive(false);

            PopulateUIElements();

            _lobbyId = _currentLobby.currentLobby.Id;
            InvokeRepeating(nameof(PollForLobbyUpdate), 1.1f, 2f);

            LobbyStatic.LogPlayersInLobby(_currentLobby.currentLobby);
        }

        private void Update()
        {
            Debug.Log(AuthenticationService.Instance.PlayerId);
        }

        private void PopulateUIElements()
        {
            lobbyName.text = "Lobby Name : " + _currentLobby.currentLobby.Name;
            lobbyCode.text = "Lobby Code : " + _currentLobby.currentLobby.LobbyCode;
            gameMode.text = "Game Mode : " + _currentLobby.currentLobby.Data["GameMode"].Value;

            ClearContainer();
            foreach (var player in _currentLobby.currentLobby.Players)
            {
                CreatePlayerInfoCard(player);
            }
        }

        private void CreatePlayerInfoCard(Player player)
        {
            var text = Instantiate(playerInfoPrefab, Vector3.zero, Quaternion.identity);
            text.name = player.Joined.ToShortTimeString();
            text.GetComponent<TextMeshProUGUI>().text = player.Id + " : " + player.Data["PlayerLevel"].Value;
            text.transform.SetParent(playerInfoPanel.transform);
        }

        private async void PollForLobbyUpdate()
        {
            _currentLobby.currentLobby = await LobbyService.Instance.GetLobbyAsync(_lobbyId);
            PopulateUIElements();
        }

        private void ClearContainer()
        {
            if (playerInfoPanel is not null && playerInfoPanel.transform.childCount > 0)
            {
                foreach (Transform VARIABLE in playerInfoPanel.transform)
                {
                    Destroy(VARIABLE.gameObject);
                }
            }
        }

        //Button Events

        public async void ChangeLobbyFeature()
        {
            string _newLobbyName = null;
            string playerName = null;

            if (newLobbyName.text != null)
            {
                _newLobbyName = newLobbyName.text;
            }

            if (newPlayerName.text != null)
            {
                playerName = newPlayerName.text;
            }

            try
            {
                if (_newLobbyName != null)
                {
                    if (_currentLobby.currentLobby.HostId == AuthenticationService.Instance.PlayerId)
                    {
                        UpdateLobbyOptions lobbyOptions = new UpdateLobbyOptions();
                        lobbyOptions.Name = _newLobbyName;
                        _currentLobby.currentLobby = await Lobbies.Instance.UpdateLobbyAsync(_lobbyId, lobbyOptions);
                    }
                }

                if (playerName != null)
                {
                    UpdatePlayerOptions playerOptions = new UpdatePlayerOptions();
                    playerOptions.Data = new Dictionary<string, PlayerDataObject>()
                    {
                        { "PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Public, playerName) }
                    };

                    await LobbyService.Instance.UpdatePlayerAsync(_lobbyId, AuthenticationService.Instance.PlayerId, playerOptions);
                }
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError(e);
            }
        }
    }
}