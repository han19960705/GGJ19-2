using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        //transform.position.Set(target.position.x, target.position.y, transform.position.z);
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }
}
