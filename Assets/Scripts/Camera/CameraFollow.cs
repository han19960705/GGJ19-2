using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField] RoleSO setting;

    public Transform target;

    float verticleScrollThreshold = 0.6f;
    float horizontalScrollThreshold = 0.5f;
    Camera cam;
    public bool freezed = false;
    BoxCollider2D col;
    int constrainLayer;

    public Vector2 minLimit;
    public Vector2 maxLimit;

    void Start() {
        cam = GetComponent<Camera>();
        col = GetComponent<BoxCollider2D>();
        col.size = new Vector2(cam.aspect, 1.0f) * 2.0f * cam.orthographicSize;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) freezed = false;
        if (Input.GetKeyDown(KeyCode.F)) freezed = true;
        if (freezed) return;

        verticleScrollThreshold = setting.verticleScrollThreshold;
        horizontalScrollThreshold = setting.horizontalScrollThreshold;
        float hw = cam.orthographicSize * cam.aspect;
        float hh = cam.orthographicSize;
        Vector3 cur = transform.position;
        Vector3 tar = target.position;
        float d = tar.x - (cur.x - hw * horizontalScrollThreshold);
        if (d < 0.0f) cur.x += d;
        d = tar.x - (cur.x + hw * horizontalScrollThreshold);
        if (d > 0.0f) cur.x += d;
        d = tar.y - (cur.y - hh * verticleScrollThreshold * 0.5f);
        if (d < 0.0f) cur.y += d;
        d = tar.y - (cur.y + hh * verticleScrollThreshold);
        if (d > 0.0f) cur.y += d;

        if (cur.x - hw < minLimit.x) cur.x = minLimit.x + hw;
        if (cur.x + hw > maxLimit.x) cur.x = maxLimit.x - hw;
        if (cur.y - hh < minLimit.y) cur.y = minLimit.y + hh;
        if (cur.y + hh > maxLimit.y) cur.y = maxLimit.y - hh;
        transform.position = cur;
    }
}
