using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item 
{

	public int Id{ get; private set;}
	public string Name{ get; private set;}
	public string Description{ get; private set;}
	public string Icon{ get; private set;}
    public string ItemType { get; protected set; }

	public item (int id, string name, string description,string icon){
		this.Id = id;
		this.Name = name;
		this.Description = description;
		this.Icon = icon;
	}
}
