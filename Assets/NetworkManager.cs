﻿using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

    private const string typeName = "UniqueGameName";
    private const string gameName = "RoomName";
   // NetworkView networkView;


    void start()
    {
        MasterServer.ipAddress = Network.player.ipAddress;
        MasterServer.port = 23466;
    }
    private void StartServer()
    {
        
       // Network.InitializeServer();
        //Network.
        //MasterServer.RegisterHost(typeName, gameName);
        Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
        
        MasterServer.RegisterHost(typeName, gameName);
    }


    void OnServerInitialized()
    {
        Debug.Log("Server Initializied");
    }


    void OnGUI()
    {
        if (!Network.isClient && !Network.isServer)
        {
            if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
                StartServer();

            if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
                RefreshHostList();



            


            if (hostList != null)
            {
                for (int i = 0; i < hostList.Length; i++)
                {
                    if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
                        JoinServer(hostList[i]);
                }
            }


        }

        if (GUI.Button(new Rect(400, 250, 250, 100), "Teste"))
            GetComponent<NetworkView>().RPC("ReceiveSimpleMessage", RPCMode.Server, "Hello world");
    }

    private HostData[] hostList;

    private void RefreshHostList()
    {
        MasterServer.RequestHostList(typeName);
    }

    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.HostListReceived)
            hostList = MasterServer.PollHostList();
    }


    private void JoinServer(HostData hostData)
    {
        Network.Connect(hostData);
    }

    [RPC]
    void ReceiveSimpleMessage(string teste,NetworkMessageInfo info)
    {
        Debug.Log(teste + " from " + info.sender);

       // if (networkView.isMine)
            
    }


    void OnConnectedToServer()
    {
        Debug.Log("Server Joined");
    }
}
