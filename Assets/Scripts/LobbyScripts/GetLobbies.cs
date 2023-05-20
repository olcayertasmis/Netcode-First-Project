using System;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace LobbyScripts
{
    public class GetLobbies : MonoBehaviour
    {
        [SerializeField] private GameObject buttonsContainer;
        [SerializeField] private GameObject buttonPrefab;

        private async void Start()
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        public async void GetLobbiesTest()
        {
            ClearContainer();

            try
            {
                QueryLobbiesOptions options = new();
                Debug.LogWarning("QueryLobbiesTest");
                options.Count = 25;

                // Filter for open lobbies only
                options.Filters = new List<QueryFilter>()
                {
                    new QueryFilter(
                        field: QueryFilter.FieldOptions.AvailableSlots,
                        op: QueryFilter.OpOptions.GT,
                        value: "0"
                    )
                };

                // Order by newest lobbies first
                options.Order = new List<QueryOrder>()
                {
                    new QueryOrder(
                        asc: false,
                        field: QueryOrder.FieldOptions.Created
                    )
                };

                QueryResponse lobbies = await Lobbies.Instance.QueryLobbiesAsync(options);
                Debug.LogWarning("Get Lobbies Done COUNT: " + lobbies.Results.Count);

                foreach (var availableLobby in lobbies.Results)
                {
                    LobbyStatic.LogLobby(availableLobby);
                    CreateLobbyButton(availableLobby);
                }

                //...
            }
            catch (LobbyServiceException e)
            {
                Debug.LogError(e);
            }
        }

        private void CreateLobbyButton(Lobby lobby)
        {
            var button = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity);
            button.name = lobby.Name + "_button";
            button.GetComponentInChildren<TextMeshProUGUI>().text = lobby.Name;
            button.transform.SetParent(buttonsContainer.transform);
            button.GetComponent<Button>().onClick.AddListener(delegate { LobbyOnClick(lobby); });
        }

        public void LobbyOnClick(Lobby lobby)
        {
            Debug.Log("Clicked Lobby : " + lobby.Name);
            GetComponent<JoinLobby>().JoinLobbyWithLobbyID(lobby.Id);
        }

        private void ClearContainer()
        {
            if (buttonsContainer is not null && buttonsContainer.transform.childCount > 0)
            {
                foreach (Transform VARIABLE in buttonsContainer.transform)
                {
                    Destroy(VARIABLE.gameObject);
                }
            }
        }
    }
} //Class