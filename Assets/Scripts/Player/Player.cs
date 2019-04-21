using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Ins { get; private set; }

    public Animator animator;
    Rigidbody2D rigidbody2;
    private void Awake()
    {
        if (Ins != null)
        {
            Destroy(this);
        }

        Ins = this;

        rigidbody2 = this.GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player Trigger " + collision.transform.tag + " " + collision.transform.name);
        if (collision.transform.CompareTag("Stinger"))
        {
            animator.SetTrigger("Die");
            AudioManager.Ins.Play("die");
            rigidbody2.Sleep();
            ////TODO : change game state to GAME OVER
            //GameManager.Ins.state = EGameState.GAMEOVER;
        }
        else if (collision.transform.CompareTag("Winner"))
        {

            //TODO : change game state to GAME Winner
            GameManager.Ins.state = EGameState.WINNER;
        }
    }

    //Animation Events
    public void GameOver()
    {
        //TODO : change game state to GAME OVER
        GameManager.Ins.state = EGameState.GAMEOVER;
    }


    public void Respawn()
    {
        animator.SetTrigger("Relive");
        this.transform.position = PlayerRecord.recordPos;
        rigidbody2.WakeUp();
    }
}
