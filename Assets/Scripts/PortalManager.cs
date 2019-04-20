using UnityEngine;
using UnityEngine.UI;

public class Tunnel {
    public Portal entry;
    public Portal target;
    public int entryWindow;
    public int targetWindow;
    public Vector3 entryCenter;
    public Vector3 entryExtents;
    public Vector3 targetCenter;
    public Vector3 targetExtents;
    public int targetPortalIdx; // for disabling the portal we came from
}

public class PortalManager : MonoBehaviour {

    public WindowManager window;
    public PlayerManager player;

    public Text textDebug;
    public Vector3 leeway = new Vector3(15, 15); // in pixels

    Tunnel _tunnel = new Tunnel();
    [HideInInspector]
    public Portal[] portals;

    int disabledIdx = -1;

    // Start is called before the first frame update
    void Start() {
        portals = GetComponentsInChildren<Portal>();
    }

    public void LeavingPortal (int idx) {
        if (disabledIdx >= 0) portals[disabledIdx].gameObject.SetActive(true);
        portals[idx].gameObject.SetActive(false);
        disabledIdx = idx;
    }

    public Tunnel GetConnectedPortal(Portal p) {
        Tunnel tunnel = _tunnel;
        tunnel.entry = p;
        tunnel.entryWindow = window.network.connID;
        bool inside = GetCenterOnScreen(tunnel.entryWindow, p, player.cameras[tunnel.entryWindow], out tunnel.entryCenter);
        if (!inside) return null;
        tunnel.entryExtents = p.GetHalfExtentsOnScreen(player.cameras[tunnel.entryWindow]);

        if (window.network.dbg_info) textDebug.text = tunnel.entryCenter + " " + tunnel.entryExtents + "\n\n";
        else textDebug.text = "";

        for (int i = 0; i < window.positions.Count; i++) {
            if (i == tunnel.entryWindow) continue;

            for (int j = 0; j < portals.Length; j++) {
                Portal portal = portals[j];
                inside = GetCenterOnScreen(i, portal, player.cameras[i], out tunnel.targetCenter);
                if (!inside) continue;
                tunnel.targetExtents = portal.GetHalfExtentsOnScreen(player.cameras[i]);

                if (window.network.dbg_info) textDebug.text += tunnel.targetCenter + " " + tunnel.targetExtents + "\n";

                tunnel.target = portal;
                tunnel.targetWindow = i;
                tunnel.targetPortalIdx = j;

                if (CloseEnough(tunnel.entryCenter, tunnel.entryExtents, tunnel.targetCenter, tunnel.targetExtents))
                    return tunnel;
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
