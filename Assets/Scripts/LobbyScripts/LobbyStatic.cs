using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace LobbyScripts
{
    public class LobbyStatic : MonoBehaviour
    {
        public static void LogPlayersInLobby(Lobby lobby)
        {
            foreach (Player player in lobby.Players)
            {
                Debug.Log("Player ID : " + player.Id);
                Debug.Log("Player Level : " + player.Data["PlayerLevel"].Value);
            }
        }
    }
}