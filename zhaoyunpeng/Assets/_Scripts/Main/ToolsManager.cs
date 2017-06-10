using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class ToolsManager : MonoBehaviour {
    public GameObject[] tools;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < tools.Length; i++)
        {
            foreach (Transform child in tools[i].transform)
            {
                if (child.gameObject.layer == 11)
                {
                    /*if (child.gameObject.GetComponent<CharacterController>().isGrounded)
                    {
                        child.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    }
                    else
                    {
                        child.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                    }*/
                }
            }
        }
	}
}
