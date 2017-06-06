#pragma strict
var attacks : AnimationClip[];
var target : GameObject;
var powerAttack : AnimationClip;
private var attacknumber : int = 0;
private var drawed : boolean = true;

function OnGUI () {
		if (GUI.Button(Rect(10,10,150,25),"random attack (3)"))
			attack();

		if (GUI.Button(Rect(170,10,150,25),"power attack"))
			powerattack();
			
			
		if (GUI.Button(Rect(330,10,150,25),"finish him"))
			finishattack();
			
		if (GUI.Button(Rect(490,10,150,25),"draw/holster "))
			drawholster();
			
	}

function attack () {
target.GetComponent.<Animation>().Play(attacks[attacknumber].name);
attacknumber ++;
if (attacknumber == 3) attacknumber=0;
yield WaitForSeconds(0.8);
target.GetComponent.<Animation>().Play("idle");
}

function powerattack () {
target.GetComponent.<Animation>().Play("powerAttack");
yield WaitForSeconds(1.7);
target.GetComponent.<Animation>().Play("idle");
}

function finishattack () {
target.GetComponent.<Animation>().Play("finishAttack");
yield WaitForSeconds(1.5);
target.GetComponent.<Animation>().Play("idle");
}

function drawholster () {
if (drawed==true) {
	target.GetComponent.<Animation>().Play("holster");
	drawed=false;
	}
else {
	target.GetComponent.<Animation>().Play("draw");
	drawed=true;
	yield WaitForSeconds(1.3);
	target.GetComponent.<Animation>().Play("idle");
	}

}