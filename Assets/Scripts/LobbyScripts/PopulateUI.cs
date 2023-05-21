using System;
using TMPro;
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

        private void Start()
        {
            _currentLobby = GameObject.Find("Lobby Manager").GetComponent<CurrentLobby>();

            PopulateUIElements();

            LobbyStatic.LogPlayersInLobby(_currentLobby.currentLobby);
        }

        private void PopulateUIElements()
        {
            lobbyName.text = "Lobby Name : " + _currentLobby.currentLobby.Name;
            lobbyCode.text = "Lobby Code : " + _currentLobby.currentLobby.LobbyCode;
            gameMode.text = "Game Mode : " + _currentLobby.currentLobby.Data["GameMode"].Value;
        }
    }
}