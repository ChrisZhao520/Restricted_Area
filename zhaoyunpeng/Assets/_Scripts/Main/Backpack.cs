using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    public GameObject backpack;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (backpack.activeInHierarchy == true)
            {
                backpack.SetActive(false);
            }
            else
            {
                backpack.SetActive(true);
            }
        }
    }
}
