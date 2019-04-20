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
        text.transform.gameObject.SetActive(false);
        btn.transform.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowGameOver()
    {
        text.transform.gameObject.SetActive(true);
        btn.transform.gameObject.SetActive(true);
    }
}
