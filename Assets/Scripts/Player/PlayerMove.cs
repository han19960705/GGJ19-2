using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    [SerializeField] RoleSO setting;

    public Animator animator;
    public Rigidbody2D rigidbody2D;
    public float jumpSpeedInit = 1;

    bool isGrounded;
    bool isJump = false;
    bool canJump = true;
    float distanceBetweenTerrain;
    float prevPosX;
    float prevPosY;
    // Update is called once per frame
    void Update()
    {
        //prev record
        prevPosX = this.transform.position.x;
        prevPosY = this.transform.position.y;

        //

        float speed = setting.moveSetting.speed * Time.deltaTime;
        float horizontal = Input.GetAxis("Horizontal");
        
        isJump = Input.GetButtonDown("Jump");

        if (isJump && canJump)
        {
            canJump = false;
            Debug.Log("here Jump");

            animator.SetTrigger("Jump");
        }

        transform.Translate(horizontal * speed, 0, 0, Space.World);

        //转向
        if (prevPosX > this.transform.position.x)
        {
            //Right
            this.transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
        }
        else if (prevPosX < this.transform.position.x)
        {
            //Left
            this.transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
        }

        //动画管理
        if (horizontal != 0)
        {
            animator.SetTrigger("Move");
        }
        else
        {
            animator.SetTrigger("Idle");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        canJump = true;
        Debug.Log("here OnCollisionEnter");
    }

    //Animation Event
    public void JumpOver(string msg)
    {
        animator.SetTrigger("JumpOver");
        rigidbody2D.AddForce(new Vector2(0, setting.moveSetting.jumpForce), ForceMode2D.Impulse);
    }
}
