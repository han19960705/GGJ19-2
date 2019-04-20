using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRecord : MonoBehaviour
{

    public static Vector3 recordPos = new Vector3();

    int prevID = 0;

    private void Awake()
    {
        if (PlayerPrefs.HasKey("recordPosX"))
        {
            recordPos.x = PlayerPrefs.GetFloat("recordPosX");
            recordPos.y = PlayerPrefs.GetFloat("recordPosY");
            recordPos.z = PlayerPrefs.GetFloat("recordPosZ");
        }
        else
        {
            PlayerPrefs.SetFloat("recordPosX", transform.position.x);
            PlayerPrefs.SetFloat("recordPosY", transform.position.y);
            PlayerPrefs.SetFloat("recordPosZ", transform.position.z);
            PlayerPrefs.Save();

            recordPos = transform.position;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5)) transform.position = PlayerRecord.recordPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetInstanceID() == prevID)
            return;

        if (collision.CompareTag("Record"))
        {
            Debug.Log("record " + collision.name);

            recordPos = collision.transform.position;
            PlayerPrefs.SetFloat("recordPosX", recordPos.x);
            PlayerPrefs.SetFloat("recordPosY", recordPos.y);
            PlayerPrefs.SetFloat("recordPosZ", recordPos.z);
            PlayerPrefs.Save();

            prevID = collision.GetInstanceID();
        }
    }
}
