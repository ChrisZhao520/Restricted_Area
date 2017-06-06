using UnityEngine;
using System.Collections;

public class kinife : MonoBehaviour {
    public AnimationClip[] attacks;
    public GameObject target;
    public AnimationClip powerAttack;
    public int attacknumber = 0;
    public bool drawed = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            attack();
        }
        if (Input.GetMouseButtonDown(1))
        {
            powerattack();
        }
	}
    public void attack() {
        target.GetComponent<Animation>().Play(attacks[attacknumber].name);
        attacknumber ++;
        if (attacknumber == 3) {
            attacknumber=0;
        }
        
        target.GetComponent<Animation>().CrossFade("idle",0.8f);
     }

    public void powerattack() {
        target.GetComponent<Animation>().Play("powerAttack");
        
        target.GetComponent<Animation>().CrossFade("idle",1.7f);
    }

    public void finishattack() {
        target.GetComponent<Animation>().Play("finishAttack");
        
        //target.GetComponent<Animation>().Play("idle");
     }

    public void drawholster() {
        if (drawed==true) {
	        target.GetComponent<Animation>().Play("holster");
	        drawed=false;
	    }
        else {
	        target.GetComponent<Animation>().Play("draw");
	        drawed=true;
	        
	        target.GetComponent<Animation>().CrossFade("idle",0.7f);
	}

}
}
