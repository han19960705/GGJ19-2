﻿using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    [SerializeField] RoleSO setting;

    public Animator animator;
    public new Rigidbody2D rigidbody2D;

    bool canJump = true;
    bool isLanding = false;

    float horizontal;

    float prevPosX;
    float prevPosY;


    // Update is called once per frame
    void Update()
    {
        if (GameManager.Ins.state != EGameState.GAMING)
            return;
        //prev record
        prevPosX = transform.position.x;
        prevPosY = transform.position.y;

        //运动
        float speed = setting.moveSetting.speed * Time.deltaTime;
        horizontal = Input.GetAxis("Horizontal");
        
        if (Input.GetButtonDown("Jump") && canJump)
        {
            Debug.Log("here Jump");
            canJump = false;
            isLanding = false;
            animator.SetBool("IsLanding", false);
            animator.SetTrigger("Jump");

            AudioManager.Ins.Play("jump");

            rigidbody2D.AddForce(new Vector2(0, setting.moveSetting.jumpForce), ForceMode2D.Impulse);
        }

        transform.Translate(horizontal * speed, 0, 0, Space.World);

        //转向
        if (prevPosX > transform.position.x)
        {
            //Right
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
        }
        else if (prevPosX < transform.position.x)
        {
            //Left
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
        }

        //动画状态管理
        if (horizontal != 0f)
        {
            if (isLanding)
            {
                animator.SetBool("IsMoving", true);
                AudioManager.Ins.Play("move");
            }
        }
        else
        {
            animator.SetBool("IsMoving", false);
            AudioManager.Ins.Stop("move");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("here OnCollisionEnter with " + collision.transform.tag + " "+ collision.transform.name);
        if (collision.transform.tag != "Box")
        {
            canJump = true;
        }
        else
        {
            if (transform.position.y > collision.transform.position.y)
            {
                canJump = true;
            }
        }

        isLanding = true;
        animator.SetBool("IsLanding", true);
    }

    //Animation Event
    public void JumpOver()
    {
        animator.SetTrigger("JumpOver");
    }

}
