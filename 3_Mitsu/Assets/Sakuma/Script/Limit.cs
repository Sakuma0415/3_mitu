using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Limit : MonoBehaviour
{

    [SerializeField]
    Text text;

    void Update()
    {
        int data = (int)Progress.Instance.LimitTime;
        if (data >= 0)
        {
            text.text = data.ToString();
        }
    }
}
