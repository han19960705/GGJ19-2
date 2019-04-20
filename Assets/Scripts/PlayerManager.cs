using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable CS0618 // Type or member is obsolete

class PlayerMessage : NSGMessage {
    public Vector3 pos;
    public Vector3 camPos;
    public int portalIdx; // only when teleporting

    public override void Deserialize(NetworkReader reader) {
        base.Deserialize(reader);
        pos = reader.ReadVector3();
        camPos = reader.ReadVector3();
        portalIdx = reader.ReadInt32();
    }

    public override void Serialize(NetworkWriter writer) {
        base.Serialize(writer);
        writer.Write(pos);
        writer.Write(camPos);
        writer.Write(portalIdx);
    }
}

public class PlayerManager : MonoBehaviour {

    public NetworkAgent network;
    public Transform[] players;
    public Camera[] cameras;
    public PortalManager portals;

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
        SendPlayerMsg(idx);
    }

    void OnConnected(NetworkMessage msg) {
        int connID = msg.conn.connectionId;
        idx = connID >= cameras.Length ? cameras.Length - 1 : connID;
        cameras[idx].gameObject.active = true;
        if (idx == 0) players[idx].gameObject.active = true; // only enable our character for the first window
        network.OnConnnected(msg); // overrided original callback
    }

    public void SendPlayerMsg(int idx, int portalIdx = 0) {
        if (!NetworkClient.active) return;
        PlayerMessage msg = new PlayerMessage {
            connID = network.connID,
            targetConnID = idx,
            pos = players[idx].position,
            camPos = cameras[idx].transform.position,
            portalIdx = portalIdx
        };
        network.Send(NSGMsgType.Player, msg);
    }

    void OnPlayerMsg(PlayerMessage msg) {
        SetPosition(msg.targetConnID, msg.pos.x, msg.pos.y, msg.pos.z);
        SetCameraPos(msg.targetConnID, msg.camPos.x, msg.camPos.y, msg.camPos.z);
        if (msg.targetConnID == network.connID) { // teleport back in
            players[msg.targetConnID].gameObject.SetActive(true);
            portals.LeavingPortal(msg.portalIdx);
        }
    }
    
    void OnGUI() {
        if (!network.dbg_info) return;
        int ypos = 0;
        GUI.Label(new Rect(300, ypos += 20, 300, 20), "Player & Camera:");
        for (int i = 0; i < players.Length; i++) {
            GUI.Label(new Rect(300, ypos += 20, 300, 20),
                players[i].position + " " + cameras[i].transform.position);
        }
    }
}
