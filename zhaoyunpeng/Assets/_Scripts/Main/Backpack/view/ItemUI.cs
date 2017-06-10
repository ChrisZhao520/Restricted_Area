using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour {

    private Sprite sp;
    private string icon;
    public Image img;
    public void updateItem(int id)
    {

        icon = "_Images" + "/" + id;
        //Debug.LogWarning(icon);
        sp = Resources.Load(icon, typeof(Sprite)) as Sprite;
        img.sprite = sp;
    }

}
