using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameStart : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        if(Input.anyKey) {
            NotificationCenter.Notify("StartGame");
            Destroy(gameObject);
        }
    }
}
