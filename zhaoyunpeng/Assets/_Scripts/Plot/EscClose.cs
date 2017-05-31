using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EscClose : MonoBehaviour {
    public MovieTexture movTexture;
    public GameObject graduallyActive;

    private float t;

	// Use this for initialization
	void Start () {
        t = 0;
	}
	
	// Update is called once per frame
	void Update () {
        t += Time.deltaTime * 0.5f;
        graduallyActive.GetComponent<Image>().color = new Color(0, 0, 0, t);
        gameObject.GetComponent<AudioSource>().volume = 1 - t;

        if (t >= 2)
        {
            movTexture.Stop();
            SceneManager.LoadScene("Main");
        }
	}
}
