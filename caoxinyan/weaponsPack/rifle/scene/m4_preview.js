#pragma strict
var target : GameObject;
private var drawed : boolean = true;
private var aiming : boolean = false;
private var playerview : boolean = true;

function Start(){

}

function OnGUI () {
		if (GUI.Button(Rect(10,10,50,25),"fire"))
			fire();

		if (GUI.Button(Rect(70,10,70,25),"burst fire"))
			burstfire();
		
		if (GUI.Button(Rect(150,10,115,25),"Aim in/out"))
			aim();	
			
		if (GUI.Button(Rect(275,10,70,25),"reload"))
			reload();	
			
		if (GUI.Button(Rect(355,10,85,25),"holster/draw"))
			draw();	
			
		if (GUI.Button(Rect(595,10,130,25),"player/world model"))
			switchview();
	}

function fire () {
target.GetComponent.<Animation>().Play("shotSingle");
target.GetComponent.<Animation>().CrossFade("idle",0.4);
}

function burstfire () {
target.GetComponent.<Animation>().Play("shotBurst");
}

function aim () {
if (!aiming) {
	GetComponent.<Animation>()["aimIN"].speed = 2;
	target.GetComponent.<Animation>().Play("aimIN");
	aiming=true;
	}
else {
	GetComponent.<Animation>()["aimOUT"].speed = 2;
	target.GetComponent.<Animation>().Play("aimOUT");
	aiming=false;
	target.GetComponent.<Animation>().CrossFade("idle",1);
	}
}


function reload () {
GetComponent.<Animation>()["reload"].speed = 1.5;
target.GetComponent.<Animation>().Play("reload");
target.GetComponent.<Animation>().CrossFade("idle",7);
}

function draw () {

if (drawed) {
	GetComponent.<Animation>()["holster"].speed = 1.5;
	target.GetComponent.<Animation>().Play("holster");
	drawed=false;
	}
else {
	GetComponent.<Animation>()["draw"].speed = 1.5;
	target.GetComponent.<Animation>().Play("draw");
	drawed=true;
	target.GetComponent.<Animation>().CrossFade("idle",1.5);
	}

}






function switchview () {
	if(playerview){
		gameObject.Find("hands").GetComponent.<Renderer>().enabled=false;
		gameObject.Find("rifle08_m4").GetComponent.<Renderer>().enabled=false;
		gameObject.Find("rifle08_charger").GetComponent.<Renderer>().enabled=false;
		gameObject.Find("m4WHD").GetComponent.<Renderer>().enabled=true;
		playerview = false;
	}
	else{
		gameObject.Find("m4WHD").GetComponent.<Renderer>().enabled=false;
		gameObject.Find("hands").GetComponent.<Renderer>().enabled=true;
		gameObject.Find("rifle08_m4").GetComponent.<Renderer>().enabled=true;
		gameObject.Find("rifle08_charger").GetComponent.<Renderer>().enabled=true;
		playerview = true;
	}
}