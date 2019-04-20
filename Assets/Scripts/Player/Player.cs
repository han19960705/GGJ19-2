using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator;

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
            this.animator.SetTrigger("Die");
            ////TODO : change game state to GAME OVER
            //GameManager.Ins.state = EGameState.GAMEOVER;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Player with " + collision.transform.tag + " " + collision.transform.name);
        if (collision.transform.CompareTag("Stinger"))
        {
            this.animator.SetTrigger("Die");
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
}
