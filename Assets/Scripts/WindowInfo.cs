using UnityEngine;
using System.Runtime.InteropServices;

public class WindowInfo : MonoBehaviour {

    const int CLIENT_LIMIT = 2;
    readonly Vector2[] positions = new Vector2[CLIENT_LIMIT];
    readonly Vector2[] sizes = new Vector2[CLIENT_LIMIT];

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
        positions[idx].x = x;
        positions[idx].y = y;
    }

    public void SetSize(int idx, float width, float height) {
        sizes[idx].x = width;
        sizes[idx].y = height;
    }

    void Update() {
        var pos = GetWindowPos();
        SetPosition(network.connID, pos.x, pos.y);
        SetSize(network.connID, Screen.width, Screen.height);
        network.SendWindowMsg(positions[network.connID], sizes[network.connID]);
    }

    void OnGUI() {
        GUI.Label(new Rect(200, 0, 300, 20), positions[0].ToString() + " " + sizes[0].ToString());
        GUI.Label(new Rect(200, 100, 300, 20), positions[1].ToString() + " " + sizes[1].ToString());
    }
}
