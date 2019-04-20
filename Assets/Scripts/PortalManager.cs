using UnityEngine;
using UnityEngine.UI;

public class PortalManager : MonoBehaviour {

    public WindowManager window;
    Portal[] portals;

    public Text textDebug;
    Vector3 leeway = new Vector3(30, 30); // in pixels

    // Start is called before the first frame update
    void Start() {
        portals = GetComponentsInChildren<Portal>();
    }

    public Portal GetConnectedPortal(Portal p) {
        int cur = window.network.connID;
        bool inside = GetCenterOnScreen(cur, p, window.network.cameras[cur], out Vector3 curCenter);
        if (!inside) return null;
        Vector3 curExtents = p.GetHalfExtentsOnScreen(window.network.cameras[cur]);

        //textDebug.text = curCenter + " " + curExtents;

        for (int i = 0; i < window.positions.Count; i++) {
            if (i == cur) continue;

            foreach (var portal in portals) {
                inside = GetCenterOnScreen(i, portal, window.network.cameras[i], out Vector3 center);
                if (!inside) continue;
                Vector3 extends = portal.GetHalfExtentsOnScreen(window.network.cameras[i]);

                //Debug.Log(center + " " + extends);

                if (CloseEnough(curCenter, curExtents, center, extends))
                    return portal;
            }

        }

        return null;
    }

    bool GetCenterOnScreen(int idx, Portal p, Camera camera, out Vector3 screen_pos) {
        bool inside = p.GetCenterInClientRect(camera, out screen_pos);
        if (!inside) return false;
        if (idx >= window.positions.Count) return false;
        Vector2 pos = window.positions[idx];
        screen_pos.x += pos.x;
        screen_pos.y += pos.y;
        return true;
    }

    bool CloseEnough(Vector3 c1, Vector3 e1, Vector3 c2, Vector3 e2) {
        Vector3 min1 = c1 - e1 - leeway;
        Vector3 max1 = c1 + e1 + leeway;
        Vector3 min2 = c2 - e2 - leeway;
        Vector3 max2 = c2 + e2 + leeway;
        return max1.x >= min2.x && max1.y >= min2.y
            && max2.x >= min1.x && max2.y >= min1.y;
    }
}
