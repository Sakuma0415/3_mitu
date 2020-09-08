using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storehouse : MonoBehaviour
{
    //instance
    public static Storehouse instance;

    public int honeyinstore;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
