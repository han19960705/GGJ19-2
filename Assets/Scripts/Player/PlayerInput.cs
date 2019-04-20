using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [System.Serializable]
    public class InputData
    {
        public float Horizontal;
        public float Vertical;
        public bool IsJump;
    }
    public InputData inputData = new InputData();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
