using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    SpriteRenderer sprite;
    PortalManager manager;
    Portal target = null;

    // Start is called before the first frame update
    void Start() {
        sprite = GetComponent<SpriteRenderer>();
        manager = GetComponentInParent<PortalManager>();
    }

    public bool GetCenterInClientRect(Camera camera, out Vector3 screen_pos) {
        screen_pos = transform.position;
        screen_pos -= camera.transform.position;
        screen_pos.x = 1.0f + screen_pos.x / (camera.orthographicSize * camera.aspect);
        screen_pos.y = 1.0f - screen_pos.y / camera.orthographicSize;
        if (screen_pos.x < -0.5f || screen_pos.x > 2.5f || screen_pos.y < -0.5f || screen_pos.x > 2.5f)
            return false; // outside current window boundary
        screen_pos.x *= camera.pixelWidth * 0.5f;
        screen_pos.y *= camera.pixelHeight * 0.5f;
        return true;
    }

    public Vector3 GetHalfExtentsOnScreen(Camera camera) {
        // get world space size (this version handles rotating correctly)
        Vector2 sprite_size = sprite.sprite.rect.size;
        Vector2 local_sprite_size = sprite_size / sprite.sprite.pixelsPerUnit;
        Vector3 world_size = local_sprite_size;
        world_size.x *= transform.lossyScale.x;
        world_size.y *= transform.lossyScale.y;

        //convert to screen space size
        Vector3 screen_size = 0.5f * world_size / camera.orthographicSize;
        screen_size.x /= camera.aspect;

        //size in pixels
        screen_size.x *= camera.pixelWidth * 0.5f;
        screen_size.y *= camera.pixelHeight * 0.5f;
        return screen_size;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (!collider.GetComponent<PlayerMove>()) return;
        Tunnel t = manager.GetConnectedPortal(this);
        if (t == null) return; // die
        // teleport away
        Vector3 dir = ScreenToWorldVector(t.entryCenter - t.targetCenter, manager.player.cameras[t.targetWindow]);
        Vector3 pos = t.target.transform.position + dir;
        manager.player.SetPosition(t.targetWindow, pos.x, pos.y, pos.z);
        manager.player.SendPlayerMsg(t.targetWindow, t.targetPortalIdx);
        collider.gameObject.SetActive(false);
        manager.LeavingPortal(t.targetPortalIdx);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.F8)) sprite.enabled = false;
        if (Input.GetKeyDown(KeyCode.F9)) sprite.enabled = true;
        Tunnel t = manager.GetConnectedPortal(this);
        if (t == null) { Disconnect(this); return; }
        Connect(this, t.target);
    }

    public Sprite active_sp;
    public Sprite inactive_sp;
    public Color disabled_color = new Color(0.6f, 0.0f, 0.0f);
    public Color enabled_color = new Color(0.0f, 0.6f, 0.0f);
    void Connect(Portal a, Portal b) {
        if (a.target == b) return;
        a.target = b;
        //a.sprite.color = b.sprite.color = enabled_color;// Color.HSVToRGB(Random.value, 0.7f, 1.0f);
        a.sprite.sprite = b.sprite.sprite = active_sp;
    }

    void Disconnect(Portal p) {
        //p.sprite.color = disabled_color;
        p.sprite.sprite = inactive_sp;
        p.target = null;
    }

    Vector3 ScreenToWorldVector(Vector3 v, Camera camera) {
        Vector3 world_vector = v;
        world_vector.x /= 0.5f * camera.pixelWidth / camera.aspect;
        world_vector.y /= 0.5f * -camera.pixelHeight;
        world_vector *= camera.orthographicSize;
        return world_vector;
    }
}
