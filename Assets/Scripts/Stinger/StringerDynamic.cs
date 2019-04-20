using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringerDynamic : StringerEntity
{
    public Rigidbody2D rigidbody2D;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Drop()
    {
        Debug.Log("Drop here");
        this.rigidbody2D.WakeUp();
    }
}
