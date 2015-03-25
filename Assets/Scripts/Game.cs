using UnityEngine;
using System.Collections;
using System;


public class Game : metaioCallback, GUIManagerListener {
	
	// game states
	private enum States{INITIAL, RUNNING, PAUSED, FINISHED, LOADING_CONFIGURATION};
	
	// current game state
	private States gameState;
	
	private bool lostTracking = true;
	
	
	public GUIManager guiManager;
	
	// if there was no tracking during LOST_TRACKING_MAX_TIMEOUT help window will be shown
	private const float LOST_TRACKING_MAX_TIMEOUT = 2.5f;
	// link to metaio web page (Unity tutorial)
	private const String LINK_TO_METAIO = "http://dev.metaio.com/sdk/getting-started/unity3d/creating-new-ar-application/";
	
	private float lostTrackingTime = 0;
	
	private float timeInGame = 0;
	
	public void Start () {
		guiManager.setListener(this);
		gameState = States.INITIAL;
	}
	
	public new void Update () {
		base.Update();
		
		if (lostTracking) {
			lostTrackingTime += Time.deltaTime;

			
			//if (lostTrackingTime >=  LOST_TRACKING_MAX_TIMEOUT)
			    // if tracking was lost for rather long time do some thing	
		}
		
		if (gameState == States.RUNNING) {
			timeInGame += Time.deltaTime;
			guiManager.setDisplayTime(timeInGame);
		}	

        if (Input.GetKey(KeyCode.Escape)) {
			System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
	}

	override protected void onTrackingEvent(String trackingEvent){
		Debug.Log(trackingEvent);
		if (trackingEvent == "ETS_FOUND"){
			onRestoredTracking();
		}	
		else {
			onLostTracking();
		}	
	}
	
	override protected void onInstantTrackingEvent(string filepath)
	{
		Debug.Log("onInstantTrackingEvent: "+filepath);
		lostTracking = true;
		lostTrackingTime = 0;
		
		GUI.enabled = false;
		
		// if succeeded, set new tracking configuration
		if (filepath.Length > 0)
		{
			onPrepareGame();
			int result = metaioSDK.setTrackingConfiguration(filepath, 1);
			Debug.Log("onInstantTrackingEvent: instant tracking configuration loaded: "+result);
		// otherwise return the game to inital state	
		} else {
			gameState = States.INITIAL;	
		}
	}
	
	// performs clean up of spawner, resets timer and position of our character
	public void onPrepareGame(){
		timeInGame = 0;		
		
	}
	
	// once tracking was lost game will paused
	public void onLostTracking(){
		lostTracking = true;
		if (gameState == States.RUNNING) gameState = States.PAUSED;
		guiManager.setTrackingControls(true);        
	}
	
	public void onRestoredTracking(){
		lostTracking = false;
		lostTrackingTime = 0;
		
		if (gameState == States.PAUSED || gameState == States.LOADING_CONFIGURATION) {  
			gameState = States.RUNNING;
			guiManager.setTrackingControls(false);
		}	
	}	
	
	
	public void onCatchingCharacter(){
		gameState = States.FINISHED;		
		
	}	
	
	public void onButtonPressed(GUIManager.Buttons button ){
		switch(button) {
			
            case(GUIManager.Buttons.TRACKING_3D) : 	metaioSDK.startInstantTracking("INSTANT_3D", "");
													GUI.enabled = false;
													lostTracking = false;
													lostTrackingTime = 0;													
													gameState = States.LOADING_CONFIGURATION;
													break;
													
			case(GUIManager.Buttons.RETRY) :		if (!lostTracking) {
														gameState = States.RUNNING;
														onPrepareGame();														
														break;
													}													
													break;
		}	
	}	
}
