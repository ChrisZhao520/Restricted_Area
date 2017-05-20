using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour {

	public Text Itemname;
	public void updateItem(string name){
        Itemname.text = name;
	}
}
