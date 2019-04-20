using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

#pragma warning disable CS0618 // Type or member is obsolete

class PlayerMessage : NSGMessage {
    public Vector3 pos;
    public Vector3 camPos;

    public override void Deserialize(NetworkReader reader) {
        base.Deserialize(reader);
        pos = reader.ReadVector3();
        camPos = reader.ReadVector3();
    }

    public override void Serialize(NetworkWriter writer) {
        base.Serialize(writer);
        writer.Write(pos);
        writer.Write(camPos);
    }
}

public class PlayerManager : MonoBehaviour {

    public NetworkAgent network;
    public Transform[] players;
    public Camera[] cameras;

    int idx;

    public void SetPosition(int idx, float x, float y, float z) {
        Vector3 p = players[idx].position;
        p.x = x; p.y = y; p.z = z;
        players[idx].position = p;
    }

    public void SetCameraPos(int idx, float x, float y, float z) {
        Vector3 p = cameras[idx].transform.position;
        p.x = x; p.y = y; p.z = z;
        cameras[idx].transform.position = p;
    }

    void Start() {
        network.RegisterHandler<PlayerMessage>(NSGMsgType.Player, OnPlayerMsg);
        network.client.RegisterHandler(MsgType.Connect, OnConnected);
    }

    void Update() {
        Transform player = players[idx];
        Transform cam = cameras[idx].transform;
        SetPosition(idx, player.position.x, player.position.y, player.position.z);
        SetCameraPos(idx, cam.position.x, cam.position.y, cam.position.z);
        SendPlayerMsg(players[idx].position, cameras[idx].transform.position);
    }

    void OnConnected(NetworkMessage msg) {
        int connID = msg.conn.connectionId;
        idx = connID >= cameras.Length ? cameras.Length - 1 : connID;
        cameras[idx].gameObject.active = true;
        players[idx].gameObject.active = true;
        network.OnConnnected(msg); // overrided original callback
    }

    void SendPlayerMsg(Vector3 pos, Vector3 camPos) {
        if (!NetworkClient.active) return;
        PlayerMessage msg = new PlayerMessage {
            connID = network.connID,
            pos = pos,
            camPos = camPos
        };
        network.Send(NSGMsgType.Player, msg);
    }

    void OnPlayerMsg(PlayerMessage msg) {
        SetPosition(msg.connID, msg.pos.x, msg.pos.y, msg.pos.z);
        SetCameraPos(msg.connID, msg.camPos.x, msg.camPos.y, msg.camPos.z);
    }

    /* */
    void OnGUI() {
        int ypos = 0;
        GUI.Label(new Rect(300, ypos += 20, 300, 20), "Player & Camera:");
        for (int i = 0; i < players.Length; i++) {
            GUI.Label(new Rect(300, ypos += 20, 300, 20),
                players[i].position + " " + cameras[i].transform.position);
        }
    }
    /* */
}
