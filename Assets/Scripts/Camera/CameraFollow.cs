using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    public Transform target;

    float verticleScrollThreshold = 0.6f;
    float horizontalScrollThreshold = 0.5f;
    Camera cam;

    private void Start() {
        cam = GetComponent<Camera>();
    }

    void Update() {
        float hw = cam.orthographicSize * cam.aspect;
        float hh = cam.orthographicSize;
        Vector3 cur = transform.position;
        Vector3 tar = target.position;
        float d = tar.x - (cur.x - hw * verticleScrollThreshold);
        if (d < 0.0f) cur.x += d;
        d = tar.x - (cur.x + hw * verticleScrollThreshold);
        if (d > 0.0f) cur.x += d;
        d = tar.y - (cur.y - hh * horizontalScrollThreshold);
        if (d < 0.0f) cur.y += d;
        d = tar.y - (cur.y + hh * horizontalScrollThreshold);
        if (d > 0.0f) cur.y += d;
        transform.position = cur;
    }
}
