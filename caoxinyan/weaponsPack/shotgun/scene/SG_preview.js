#pragma strict
var pumplist : AnimationClip[];
var target : GameObject;
private var pumpnumber : int = 0;
private var drawed : boolean = true;
private var playerview : boolean = true;

private var zoomIn : boolean = true;
private var zoomStart : boolean;
private var unZoom : boolean;

function Update(){
if (zoomStart){
	GetComponent.<Camera>().fieldOfView = GetComponent.<Camera>().fieldOfView-1;
	if (GetComponent.<Camera>().fieldOfView<31) zoomStart = false;
}

if (unZoom){
	GetComponent.<Camera>().fieldOfView = GetComponent.<Camera>().fieldOfView+1;
	if (GetComponent.<Camera>().fieldOfView>49.1) unZoom = false;

}


}

function OnGUI () {
		if (GUI.Button(Rect(10,10,120,25),"random fire (3)"))
			fire();

		if (GUI.Button(Rect(140,10,70,25),"reload"))
			reload();
		
		if (GUI.Button(Rect(220,10,115,25),"draw/holster "))
			drawholster();	
			
		if (GUI.Button(Rect(345,10,30,25),"run "))
			run();	
			
		if (GUI.Button(Rect(385,10,110,25),"Aim in & out"))
			aimInOut();
			
		if (GUI.Button(Rect(505,10,80,25),"Aim shot"))
			aimshot();
			
		if (GUI.Button(Rect(595,10,130,25),"player/world model"))
			switchview();
	}

function fire () {
GetComponent.<Camera>().fieldOfView = 50;
target.GetComponent.<Animation>().Play("fire");
yield WaitForSeconds(0.3);
target.GetComponent.<Animation>().Play(pumplist[pumpnumber].name);
pumpnumber ++;
if (pumpnumber == 2) pumpnumber=0;
yield WaitForSeconds(0.8);
target.GetComponent.<Animation>().Play("idle");
}

function reload () {
GetComponent.<Camera>().fieldOfView = 50;
target.GetComponent.<Animation>().Play("reloadStart");
target.GetComponent.<Animation>().CrossFade("reloadCycle",0.65);
}

function run () {
GetComponent.<Camera>().fieldOfView = 50;
target.GetComponent.<Animation>().Play("runStart");
target.GetComponent.<Animation>().CrossFade("running",0.5);
}

function aimInOut () {
if (zoomIn && playerview) {
	target.GetComponent.<Animation>().Play("aimIn");
	zoomIn=false;
	yield WaitForSeconds(0.5);
	zoomStart = true;
	GetComponent.<Animation>().Play("camAimIn");
	}
else if(!zoomIn && playerview){
	target.GetComponent.<Animation>().Play("aimOut");
	zoomIn=true;
	unZoom=true;
	GetComponent.<Animation>().Play("camAimOut");
	}

}

function drawholster () {
GetComponent.<Camera>().fieldOfView = 50;
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
function aimshot () {
if (!zoomIn && playerview){
	target.GetComponent.<Animation>().Play("aimFire");
	target.GetComponent.<Animation>().CrossFade("aimPump",0.2);
}
else if (zoomIn && playerview){
	target.GetComponent.<Animation>().Play("aimIn");
	zoomIn=false;
	yield WaitForSeconds(0.5);
	zoomStart = true;
	GetComponent.<Animation>().Play("camAimIn");
	yield WaitForSeconds(0.75);
	target.GetComponent.<Animation>().Play("aimFire");
	target.GetComponent.<Animation>().CrossFade("aimPump",0.2);
}
}

function switchview () {
	if(playerview){
		gameObject.Find("SG_HP").GetComponent.<Renderer>().enabled=true;
		gameObject.Find("hands").GetComponent.<Renderer>().enabled=false;
		gameObject.Find("shell").GetComponent.<Renderer>().enabled=false;
		gameObject.Find("SGmesh").GetComponent.<Renderer>().enabled=false;
		playerview = false;
	}
	else{
		gameObject.Find("SG_HP").GetComponent.<Renderer>().enabled=false;
		gameObject.Find("hands").GetComponent.<Renderer>().enabled=true;
		gameObject.Find("shell").GetComponent.<Renderer>().enabled=true;
		gameObject.Find("SGmesh").GetComponent.<Renderer>().enabled=true;
		playerview = true;
	}
}