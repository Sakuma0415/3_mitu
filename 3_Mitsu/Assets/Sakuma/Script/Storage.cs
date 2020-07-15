using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Storage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        texts.text = machine.end.ToString();
    }

    [SerializeField]
    Machine machine;
    [SerializeField]
    Text texts;


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Player")
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                
                ItemList.Instance.okane += machine.end * 1000;
                machine.end = 0;

            }
        }
    }
}
