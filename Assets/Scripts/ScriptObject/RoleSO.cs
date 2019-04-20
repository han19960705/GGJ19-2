using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Setting", menuName = "Setting/RoleSetting")]
public class RoleSO : ScriptableObject {

    [System.Serializable]
    public class MoveSetting
    {
        public float speed;
        public float jumpHeight;
        public float jumpForce;
    }

    //[Header("move setting")]
    public MoveSetting moveSetting;
}
