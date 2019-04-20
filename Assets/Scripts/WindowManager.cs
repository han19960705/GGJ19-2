using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.Networking;
using System.Collections.Generic;

#pragma warning disable CS0618 // Type or member is obsolete

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

public class WindowManager : MonoBehaviour {

    [HideInInspector]
    public List<Vector2> positions = new List<Vector2>();
    [HideInInspector]
    public List<Vector2> sizes = new List<Vector2>();

    public NetworkAgent network;

    // https://answers.unity.com/questions/915069/want-to-get-the-location-of-the-gameview-window.html
    #if UNITY_STANDALONE_WIN || UNITY_EDITOR
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT {
            public int X;
            public int Y;
            public static implicit operator Vector2(POINT p) {
                return new Vector2(p.X, p.Y);
            }
        }
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out POINT lpPoint);
        public static Vector2 GetWindowPos() {
            Vector2 inputCursor = Input.mousePosition;
            // flip input Cursor y (as the Reference "0" is the last scanline)
            inputCursor.y = Screen.height - 1 - inputCursor.y;
            GetCursorPos(out POINT p);
            return p - inputCursor;
        }
    #endif

    public void SetPosition(int idx, float x, float y) {
        while (positions.Count <= idx) positions.Add(new Vector2());
        Vector2 p = positions[idx];
        p.x = x; p.y = y;
        positions[idx] = p;
    }

    public void SetSize(int idx, float width, float height) {
        while (sizes.Count <= idx) sizes.Add(new Vector2());
        Vector2 s = sizes[idx];
        s.x = width; s.y = height;
        sizes[idx] = s;
    }
    
    void Start() {
        network.RegisterHandler<WindowMessage>(NSGMsgType.Window, OnWindowMsg);
    }

    void Update() {
        var pos = GetWindowPos();
        SetPosition(network.connID, pos.x, pos.y);
        SetSize(network.connID, Screen.width, Screen.height);
        SendWindowMsg(positions[network.connID], sizes[network.connID]);
    }

    void SendWindowMsg(Vector2 pos, Vector2 size) {
        if (!NetworkClient.active) return;
        WindowMessage msg = new WindowMessage {
            connID = network.connID,
            pos = pos,
            size = size
        };
        network.Send(NSGMsgType.Window, msg);
    }

    void OnWindowMsg(WindowMessage msg) {
        SetPosition(msg.connID, msg.pos.x, msg.pos.y);
        SetSize(msg.connID, (int)msg.size.x, (int)msg.size.y);
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
