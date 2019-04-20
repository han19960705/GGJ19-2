using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable CS0618 // Type or member is obsolete

class NSGMsgType {
    public static short Window = MsgType.Highest + 1;
    public static short Player = MsgType.Highest + 2;
}

public class NSGMessage : MessageBase {
    public int connID;

    public override void Deserialize(NetworkReader reader) {
        base.Deserialize(reader);
        connID = reader.ReadInt32();
    }

    public override void Serialize(NetworkWriter writer) {
        base.Serialize(writer);
        writer.Write(connID);
    }
}

public class NetworkAgent : MonoBehaviour {

    NetworkManager manager;
    NetworkManagerHUD hud;
    [HideInInspector]
    public int connID;
    [HideInInspector]
    public NetworkClient client;

    // the first instance start as host, rest instances start as clients
    void Awake() {
        manager = GetComponent<NetworkManager>();
        hud = GetComponent<NetworkManagerHUD>();
        client = manager.StartHost();
        if (!NetworkClient.active) client = manager.StartClient();
        client.RegisterHandler(MsgType.Connect, OnConnnected);
    }

    void Update() {
        if (!ClientScene.ready && manager.client.connection != null) {
            ClientScene.Ready(manager.client.connection);
        }
        if (Input.GetKeyDown(KeyCode.F10)) hud.showGUI = false;
        if (Input.GetKeyDown(KeyCode.F11)) hud.showGUI = true;
        if (!client.isConnected) {
            var clients = NetworkClient.allClients;
            if (clients.Count > 0) client = clients[0];
        }
    }

    public void OnConnnected(NetworkMessage msg) {
        connID = msg.conn.connectionId;
    }

    public bool Send(short msgType, NSGMessage msg) {
        if (ClientScene.ready) return client.Send(msgType, msg);
        return false;
    }

    public void RegisterHandler<TMsg>(short msgType, Action<TMsg> callback) where TMsg : NSGMessage, new() {
        NetworkServer.RegisterHandler(msgType, OnServerMsg<TMsg>);
        client.RegisterHandler(msgType, ClientMsgCallbackFactory(callback));
    }

    NetworkMessageDelegate ClientMsgCallbackFactory<TMsg>(Action<TMsg> callback) where TMsg : NSGMessage, new() {
        return netMsg => {
            TMsg msg = netMsg.ReadMessage<TMsg>();
            if (msg.connID == connID) return; // no-op for self-posted messages
            callback(msg);
        };
    }

    void OnServerMsg<TMsg>(NetworkMessage netMsg) where TMsg : NSGMessage, new() {
        TMsg msg = netMsg.ReadMessage<TMsg>();
        NetworkServer.SendToAll(netMsg.msgType, msg); // reroute to all clients
    }
}
