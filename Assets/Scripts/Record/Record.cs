using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Record : MonoBehaviour
{
    BoxCollider2D boxCollider;
    // Start is called before the first frame update
    void Start()
    {
        tag = "Record";

        boxCollider = gameObject.AddComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
    }
}
