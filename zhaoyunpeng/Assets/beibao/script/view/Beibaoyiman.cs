using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Beibaoyiman : MonoBehaviour {

    
    public Text ContentText;

    public void Updatexianshi(string text)
    {
        ContentText.text = text;
    }
    public void show()
    {
        gameObject.SetActive(true);
    }
    public void hidden()
    {
        gameObject.SetActive(false);
    }
}
