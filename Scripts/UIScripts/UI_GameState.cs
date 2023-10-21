using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameState : MonoBehaviour
{
    public Player player;
    public Text hp;
    public Text atk;
    public Text guard;

    // Update is called once per frame
    void Update()
    {
        hp.text = "HP:" + player.hp;
        atk.text = "ATK:" + player.atk;
        guard.text = "GUARD:" + player.guard;
    }
}
