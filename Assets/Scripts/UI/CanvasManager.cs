using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Ins { get; private set; }

    public Text text;
    public Button btn;

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
        text.transform.gameObject.SetActive(flag);
        btn.transform.gameObject.SetActive(flag);
    }

    public void OnBtnClicked()
    {
        ShowGameOver(false);
        GameManager.Ins.Respawn();
    }
}
