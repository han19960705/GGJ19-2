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

    [HideInInspector]
    public List<Vector3> positions = new List<Vector3>();
    [HideInInspector]
    public List<Vector3> cameraPos = new List<Vector3>();

    public NetworkAgent network;
    public Transform player;
    public Transform cam;

    public void SetPosition(int idx, float x, float y, float z) {
        while (positions.Count <= idx) positions.Add(new Vector3());
        Vector3 p = positions[idx];
        p.x = x; p.y = y; p.z = z;
        positions[idx] = p;
    }

    public void SetCameraPos(int idx, float x, float y, float z) {
        while (cameraPos.Count <= idx) cameraPos.Add(new Vector3());
        Vector3 p = cameraPos[idx];
        p.x = x; p.y = y; p.z = z;
        cameraPos[idx] = p;
    }

    void Start() {
        network.RegisterHandler<PlayerMessage>(NSGMsgType.Player, OnPlayerMsg);
    }

    void Update() {
        SetPosition(network.connID, player.position.x, player.position.y, player.position.z);
        SetCameraPos(network.connID, cam.position.x, cam.position.y, cam.position.z);
        SendPlayerMsg(positions[network.connID], cameraPos[network.connID]);
    }

    void SendPlayerMsg(Vector3 pos, Vector3 camPos) {
        if (!NetworkClient.active) return;
        PlayerMessage msg = new PlayerMessage {
            pos = pos,
            camPos = camPos
        };
        network.Send(NSGMsgType.Window, msg);
    }

    void OnPlayerMsg(PlayerMessage msg) {
        SetPosition(msg.connID, msg.pos.x, msg.pos.y, msg.pos.z);
        SetCameraPos(msg.connID, msg.camPos.x, msg.camPos.y, msg.camPos.z);
    }

    /* *
    void OnGUI() {
        int ypos = 0;
        for (int i = 0; i < positions.Count; i++) {
            GUI.Label(new Rect(200, ypos += 20, 300, 20), 
                positions[i] + " " + sizes[i]);
        }
    }
    /* */
}
