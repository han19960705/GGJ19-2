using System;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable CS0618 // Type or member is obsolete
public class NetworkAgent : MonoBehaviour {

    NetworkManager manager;
    NetworkClient client;

    public WindowInfo windowManager;
    [HideInInspector]
    public int connID;

    class NSGMsgType {
        public static short Window = MsgType.Highest + 1;
    }

    class NSGMessage : MessageBase {
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

    class WindowMessage : NSGMessage {
        public Vector2 pos;
        public Vector2 size;

        public override void Deserialize(NetworkReader reader) {
            base.Deserialize(reader);
            pos = reader.ReadVector2();
            size = reader.ReadVector2();
        }

        public override void Serialize(NetworkWriter writer) {
            base.Serialize(writer);
            writer.Write(pos);
            writer.Write(size);
        }
    }

    // the first instance start as host, rest instances start as clients
    void Start() {
        manager = GetComponent<NetworkManager>();
        client = manager.StartHost();
        if (!NetworkClient.active) client = manager.StartClient();
        if (!ClientScene.ready) ClientScene.Ready(manager.client.connection);
        client.RegisterHandler(MsgType.Connect, OnConnnected);
        RegisterHandler<WindowMessage>(NSGMsgType.Window, OnWindowMsg);
    }

    void OnConnnected(NetworkMessage msg) {
        connID = msg.conn.connectionId;
    }

    public void SendWindowMsg(Vector2 pos, Vector2 size) {
        if (!NetworkClient.active) return;
        WindowMessage msg = new WindowMessage {
            connID = connID,
            pos = pos,
            size = size
        };
        client.Send(NSGMsgType.Window, msg);
    }

    void OnWindowMsg(WindowMessage msg) {
        windowManager.SetPosition(msg.connID, msg.pos.x, msg.pos.y);
        windowManager.SetSize(msg.connID, (int)msg.size.x, (int)msg.size.y);
    }

    /////////////
    
    void RegisterHandler<TMsg>(short msgType, Action<TMsg> callback) where TMsg : NSGMessage, new() {
        NetworkServer.RegisterHandler(NSGMsgType.Window, OnServerMsg<TMsg>);
        client.RegisterHandler(NSGMsgType.Window, ClientMsgCallbackFactory(callback));
    }

    NetworkMessageDelegate ClientMsgCallbackFactory<TMsg>(Action<TMsg> callback) where TMsg : NSGMessage, new() {
        return netMsg => {
            TMsg msg = netMsg.ReadMessage<TMsg>();
            if (msg.connID == connID) return;
            callback(msg);
        };
    }

    void OnServerMsg<TMsg>(NetworkMessage netMsg) where TMsg : NSGMessage, new() {
        TMsg msg = netMsg.ReadMessage<TMsg>();
        NetworkServer.SendToAll(NSGMsgType.Window, msg);
    }
}
#pragma warning restore CS0618 // Type or member is obsolete
