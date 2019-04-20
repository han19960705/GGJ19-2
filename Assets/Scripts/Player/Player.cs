using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Ins { get; private set; }

    public Animator animator;

    Vector3 initPos;

    private void Awake()
    {
        if (Ins != null)
        {
            Destroy(this);
        }

        Ins = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        initPos = transform.position;
        PlayerRecord.recordPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5)) transform.position = PlayerRecord.recordPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player Trigger " + collision.transform.tag + " " + collision.transform.name);
        if (collision.transform.CompareTag("Stinger"))
        {
            animator.SetTrigger("Die");
            ////TODO : change game state to GAME OVER
            //GameManager.Ins.state = EGameState.GAMEOVER;
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
    }
}
