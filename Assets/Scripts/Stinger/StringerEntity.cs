using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringerEntity : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Untagged")
        {
            Debug.Log("Stringger trigger with untagger");
        }
    }
}
