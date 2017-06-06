using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class GunHand : MonoBehaviour
{
    public GameObject gameobj;

    private bool aiming = false;
    private bool drawed = true;
    private bool playerview = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetMouseButtonDown(0)){
            if (!aiming)
            {
                burstfire();
            }
            /*else
            {
                fire();
            }*/
        }
        else if (Input.GetMouseButtonUp(0) || aiming)
        {
            gameobj.GetComponent<Animation>().Stop("shotBurst");
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            draw();
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            jump();
        }
        if (Input.GetMouseButtonDown(1))
        {
            aim();
        }
        if(Input.GetKeyDown(KeyCode.R)){
            reload();
        }
        

	}
    /*public void fire()
    {
        gameobj.GetComponent<Animation>().Play("shotSingle");
        gameobj.GetComponent<Animation>().CrossFade("idle", 0.4f);
    }*/
    public void burstfire()
    {
        gameobj.GetComponent<Animation>().Play("shotBurst");
    }
    public void aim()
    {
        if(!aiming){
            GetComponent<Animation>()["aimIN"].speed = 3;
            gameobj.GetComponent<Animation>().Play("aimIN");
            aiming = true;
        }
        else
        {
            GetComponent<Animation>()["aimOUT"].speed = 3;
            gameobj.GetComponent<Animation>().Play("aimOUT");
            aiming = false;
            gameobj.GetComponent<Animation>().CrossFade("idle", 1);
        }
    }
    public void reload()
    {
        GetComponent<Animation>()["reload"].speed = 1.5f;
        gameobj.GetComponent<Animation>().Play("reload");
        gameobj.GetComponent<Animation>().CrossFade("idle", 7);
    }
    public void draw()
    {
        if (drawed) {
	    GetComponent<Animation>()["holster"].speed = 5f;
	    gameobj.GetComponent<Animation>().Play("holster");
	    drawed=false;
	    }
        else {
	        GetComponent<Animation>()["draw"].speed = 1.5f;
	        gameobj.GetComponent<Animation>().Play("draw");
	        drawed=true;
	        gameobj.GetComponent<Animation>().CrossFade("idle",1.5f);
	     }
     }
    public void jump()
    {
        GetComponent<Animation>()["friendlyAimIn"].speed = 4f;
        gameobj.GetComponent<Animation>().Play("friendlyAimIn");

    }
}
