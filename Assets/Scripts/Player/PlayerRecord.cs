using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRecord : MonoBehaviour
{
    public static Vector3 recordPos;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Record"))
        {
            recordPos = collision.transform.position;
        }
    }
}
