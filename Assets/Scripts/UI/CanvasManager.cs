using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Ins { get; private set; }

    public Transform img;
    public Transform btn;

    public Transform txt;

    private void Awake()
    {
        if(Ins != null)
        {
            Destroy(this);
        }

        Ins = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        ShowGameOver(false);
    }
    
    public void ShowGameOver(bool flag = true)
    {
        img.gameObject.SetActive(flag);
        btn.gameObject.SetActive(flag);
    }

    public void ShowGameWinner(bool flag = true)
    {
        txt.gameObject.SetActive(flag);
    }

    public void OnBtnClicked()
    {
        ShowGameOver(false);
        GameManager.Ins.Respawn();
    }
}
