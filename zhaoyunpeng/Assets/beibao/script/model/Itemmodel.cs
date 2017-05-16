using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Itemmodel 
{
	private static Dictionary<string,item> GridItem = new Dictionary<string,item> ();
    public static void StoreItem(string name, item item)
    {
		if (GridItem.ContainsKey (name)) {
            DeleteItem(name);
		}
        GridItem.Add(name, item);
	}
    public static item GetItem(string name)
    {
        if (GridItem.ContainsKey(name))
        {
            return GridItem[name];
        }
        else
        {
            return null;
        }
    }
	public static void DeleteItem(string name){
        if (GridItem.ContainsKey(name))
        {
            GridItem.Remove(name);
        }
	}

}
