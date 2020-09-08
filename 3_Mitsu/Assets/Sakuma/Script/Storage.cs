using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Storage : MonoBehaviour
{
    bool sw = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        texts.text = machine.end.ToString();
        if (Input.GetKeyDown(KeyCode.Space)&&sw)
        {

            ItemList.Instance.okane += machine.end * 1500+(machine.end* machine.end*100);
            machine.end = 0;

        }
    }

    [SerializeField]
    Machine machine;
    [SerializeField]
    Text texts;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Player")
        {
            sw = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Player")
        {
            sw = false;
        }
    }



}
