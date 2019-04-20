using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable CS0618 // Type or member is obsolete

public class CameraController : NetworkBehaviour {


    void Start() {
        if (isClient) gameObject.active = false;
    }

    void Update() {
        Vector3 pos = transform.position;
        pos.x += 0.1f;
        transform.position = pos;
    }
}
