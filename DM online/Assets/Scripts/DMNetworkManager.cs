using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using DuelMasters;

public class DMNetworkManager : NetworkManager {
    private List<Player> allPlayers;
    public IEnumerable<Player> AllPlayers => allPlayers;
    public GameObject[] GetAllPlayerGameObjects() {
        GameObject[] allPlayerObjects = new GameObject[allPlayers.Count];
        for (int i = 0; i < allPlayers.Count; i++) {
            allPlayerObjects[i] = allPlayers[i].gameObject;
        }
        return allPlayerObjects;
    }
    public Player this[int index] => allPlayers[index];

    public override void OnStartServer() {
        allPlayers = new List<Player>();
        base.OnStartServer();
    }

    public override void OnClientSceneChanged(NetworkConnection conn) {
        base.OnClientSceneChanged(conn);
        if (networkSceneName == "Game") {
            foreach (Player player in FindObjectsOfType<Player>()) {
                player.Init();
            }
        }
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
        base.OnServerAddPlayer(conn, playerControllerId);
        foreach (NetworkInstanceId id in conn.clientOwnedObjects) {
            Player player = NetworkServer.FindLocalObject(id).GetComponent<Player>();
            if (player != null) {
                allPlayers.Add(player);
                break;
            }
        }
    }

    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController pc) {
        foreach (NetworkInstanceId id in conn.clientOwnedObjects) {
            Player player = NetworkServer.FindLocalObject(id).GetComponent<Player>();
            if (player != null) {
                allPlayers.Remove(player);
                break;
            }
        }
        base.OnServerRemovePlayer(conn, pc);
    }

    public override void OnClientDisconnect(NetworkConnection conn) {
        allPlayers.Clear();
        base.OnClientDisconnect(conn);
    }

}
