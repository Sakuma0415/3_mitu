using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMitsuList : MonoBehaviour
{
    public GameObject itemTemplate;

    public GameObject content;

    public void AddButton_Click()
    {
        var copy = Instantiate(itemTemplate);
        copy.transform.parent = content.transform;
    }
}
