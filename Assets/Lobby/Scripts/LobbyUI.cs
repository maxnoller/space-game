using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

namespace SpaceGame.UI.Lobby{
public class LobbyUI : NetworkBehaviour
{
    [SerializeField]private GameObject startButton;
    private NetworkList<PlayerState> lobbyPlayers = new NetworkList<PlayerState>();

    public override void OnNetworkSpawn(){
        if(IsClient)
            lobbyPlayers.OnListChanged += handleLobbyPlayersChanged;
        if(IsServer){
            lobbyPlayers.OnListChanged += serverHandleLobbyPlayersChanged;
            NetworkManager.Singleton.OnClientConnectedCallback += handleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += handleClientDisconnected;

            foreach(var client in NetworkManager.Singleton.ConnectedClients){
                handleClientConnected(client.Key);
            }
        }
    }

    private void OnDestroy(){
        lobbyPlayers.OnListChanged -= handleLobbyPlayersChanged;

        if(NetworkManager.Singleton){
            NetworkManager.Singleton.OnClientConnectedCallback -= handleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= handleClientDisconnected;
        }
    }

    private bool isEveryoneReady(){
        if(lobbyPlayers.Count < 2){
            return false;
        }

        foreach(PlayerState player in lobbyPlayers){
            if(!player.IsReady) return false;
        }

        return true;
    }

    private void handleClientConnected(ulong clientId)
    {
        throw new NotImplementedException();
    }
        private void handleClientDisconnected(ulong obj)
        {
            throw new NotImplementedException();
        }



        private void serverHandleLobbyPlayersChanged(NetworkListEvent<PlayerState> changeEvent)
        {
            throw new NotImplementedException();
        }

        private void handleLobbyPlayersChanged(NetworkListEvent<PlayerState> changeEvent)
        {
            throw new NotImplementedException();
        }
    }
}