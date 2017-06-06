#pragma strict
var pumplist : AnimationClip[];
var target : GameObject;
private var pumpnumber : int = 0;
private var drawed : boolean = true;
private var playerview : boolean = true;

var silencerDisplay : GameObject;

function Start(){
silencerDisplay.GetComponent.<Renderer>().enabled=false;
target.GetComponent.<Animation>().CrossFade("idleAct",0.4);
yield WaitForSeconds(4);
target.GetComponent.<Animation>().CrossFade("idle",0.4);

}

function OnGUI () {
		if (GUI.Button(Rect(10,10,120,25),"fire"))
			fire();

		if (GUI.Button(Rect(140,10,70,25),"reload"))
			reload();
		
		if (GUI.Button(Rect(220,10,115,25),"silencer on/off"))
			silencer();	
			
		if (GUI.Button(Rect(345,10,30,25),"run"))
			run();	
			
		if (GUI.Button(Rect(595,10,130,25),"player/world model"))
			switchview();
	}

function fire () {
GetComponent.<Camera>().fieldOfView = 50;
target.GetComponent.<Animation>().Play("fire2");
target.GetComponent.<Animation>().CrossFade("idle",0.4);
}

function reload () {
GetComponent.<Camera>().fieldOfView = 50;
target.GetComponent.<Animation>().Play("reload");
yield WaitForSeconds(2.2);
target.GetComponent.<Animation>().CrossFade("idle");
}

function run () {
GetComponent.<Camera>().fieldOfView = 50;
target.GetComponent.<Animation>().Play("runStart");
target.GetComponent.<Animation>().CrossFade("run",0.5);
}


function silencer () {
GetComponent.<Camera>().fieldOfView = 50;
if (drawed==true) {
	target.GetComponent.<Animation>()["silencerOn"].speed = 1.3;
	target.GetComponent.<Animation>().Play("silencerOn");
	silencerDisplay.GetComponent.<Renderer>().enabled=true;
	yield WaitForSeconds(4);
	target.GetComponent.<Animation>().Play("idle");
	drawed=false;
	}
else {
	target.GetComponent.<Animation>().Play("silencerOff");
	drawed=true;
	yield WaitForSeconds(3.5);
	target.GetComponent.<Animation>().CrossFade("idle");
	silencerDisplay.GetComponent.<Renderer>().enabled=false;
	}
}

function switchview () {
	if(playerview){
		gameObject.Find("gunHD").GetComponent.<Renderer>().enabled=true;
		gameObject.Find("hands").GetComponent.<Renderer>().enabled=false;
		gameObject.Find("gun").GetComponent.<Renderer>().enabled=false;
		gameObject.Find("topshell").GetComponent.<Renderer>().enabled=false;
		gameObject.Find("silencer").GetComponent.<Renderer>().enabled=false;
		gameObject.Find("charger").GetComponent.<Renderer>().enabled=false;
		playerview = false;
	}
	else{
		gameObject.Find("gunHD").GetComponent.<Renderer>().enabled=false;
		gameObject.Find("hands").GetComponent.<Renderer>().enabled=true;
		gameObject.Find("gun").GetComponent.<Renderer>().enabled=true;
		gameObject.Find("topshell").GetComponent.<Renderer>().enabled=true;
		gameObject.Find("silencer").GetComponent.<Renderer>().enabled=true;
		gameObject.Find("charger").GetComponent.<Renderer>().enabled=true;
		playerview = true;
	}
}