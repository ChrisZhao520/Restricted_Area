using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedLighting : MonoBehaviour {

    public void OnEnable()
    {
        Messenger<bool>.AddListener("Morning Light Time", OnToggleLight);
    }

    public void OnDisable()
    {
        Messenger<bool>.RemoveListener("Morning Light Time", OnToggleLight);
    }

    private void OnToggleLight(bool b)
    {
        if (b)
        {
            GetComponent<Light>().enabled = false;
            foreach (Transform child in this.transform)
            {
                child.GetComponent<MeshRenderer>().enabled = true;
            }
        }
        else
        {
            GetComponent<Light>().enabled = true;
            foreach (Transform child in this.transform)
            {
                child.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}
