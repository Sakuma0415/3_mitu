using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyMachine : MonoBehaviour
{
    //instances
    public Player player;
    public Storehouse store;
    public static HoneyMachine instance;

    [SerializeField]
    public float time;

    public int honeyinmachine;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        player = FindObjectOfType<Player>();
        store = FindObjectOfType<Storehouse>();
    }

    void Update()
    {
    }

    //蜂蜜製造機に近づいてスペースキーで持っている蜂国をすべて製造機に入れる(Collider)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                MakeHoney();
            }
        }
    }


    public void MakeHoney()
    {
        //製造機は時間をかけて蜂蜜を生成
        player.takenhoney = honeyinmachine;
        player.takenhoney = 0;
        time -= Time.deltaTime;
        if(time == 0)
        {
            //完成した蜂蜜は保管庫に追加する
            store.honeyinstore += honeyinmachine;
            honeyinmachine = 0;
        }
    }
}
