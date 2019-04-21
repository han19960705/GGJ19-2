using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Setting", menuName = "Setting/RoleSetting")]
public class RoleSO : ScriptableObject {

    [System.Serializable]
    public class MoveSetting
    {
        public float speed;
        public float jumpForce;
    }

    public MoveSetting moveSetting;


    [Header("camera setting")]

    public float verticleScrollThreshold = 0.6f;
    public float horizontalScrollThreshold = 0.3f;
}
