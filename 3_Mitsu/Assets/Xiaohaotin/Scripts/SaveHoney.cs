using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveHoney : MonoBehaviour
{
    //instances
    public Player player;
    public static HoneyMachine instance;
    public Store Store;

    public int honeyinmachine;
    void Start()
    {
        player = FindObjectOfType<Player>();
        Store = FindObjectOfType<Store>();
    }

    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                Savehoney();
            }
        }
    }


    public void Savehoney()
    {
        player.takenhoney = honeyinmachine;
        player.takenhoney = 0;

        Store.honeyinstore += honeyinmachine;
        honeyinmachine = 0;
    }
}
