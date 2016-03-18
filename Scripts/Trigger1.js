#pragma strict

private var guiShow : boolean = false;

var riddle : Texture;

function OnTriggerStay (Col : Collider)
{
	if(Col.tag == "Player")
	{
		guiShow = true;
	}
}

function OnTriggerExit (Col : Collider)
{
	if(Col.tag == "Player")
	{
		guiShow = false;
	}
}

function OnGUI()
{
	if(guiShow == true)
	{
		GUI.DrawTexture(Rect(Screen.width / 4.5, Screen.height / 4, 1024, 512), riddle);
	}
}