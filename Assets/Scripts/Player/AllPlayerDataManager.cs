using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class AllPlayerDataManager : NetworkBehaviour
{
    public static AllPlayerDataManager Instance;

    private NetworkList<PlayerData> allPlayerData;
    private const int LIFEPOINTS = 10;

    private void Awake()
    {
        allPlayerData = new NetworkList<PlayerData>();

        if(Instance != null && Instance != this)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    public override void OnNetworkSpawn()
    {
        if(IsServer)
        {
            AddNewClientToList(NetworkManager.LocalClientId);
        }
    }

    void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += AddNewClientToList;

    }

    void AddNewClientToList(ulong clientID)
    {
        if (IsServer) return;

        foreach(var playerData in allPlayerData)
        {
            if (playerData.clientID == clientID) return;
        }
        PlayerData newPlayerData = new PlayerData();
        newPlayerData.clientID = clientID;
        newPlayerData.score = 0;
        newPlayerData.lifePoints = LIFEPOINTS;
        newPlayerData.playerPlaced = false;

        if (allPlayerData.Contains(newPlayerData)) return;
        allPlayerData.Add(newPlayerData);
        PrintAllPlayerList();
    }

    void PrintAllPlayerList()
    {
        foreach(var playerData in allPlayerData)
        {
            Debug.Log(message: "Player ID =>" + playerData.clientID + "hasPlaced" + playerData.playerPlaced + "Called by" + NetworkManager.Singleton.LocalClientId);

        }
    }


   
    void Update()
    {
        
    }
}
