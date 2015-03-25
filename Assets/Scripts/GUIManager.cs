using UnityEngine;
using System.Collections;
using System;

public interface GUIManagerListener {
    void onButtonPressed(GUIManager.Buttons button);
}

/// <summary>
///  GUI Manager is responsible for rendering GUI content such as buttons and text in a scene
/// and notifiying controller (GUIManagerListener) about GUI events
/// </summary>
public class GUIManager : MonoBehaviour {
	
	public enum Buttons {FORWARD, BACK, RIGHT, LEFT, HELP, TRACKING_2D, TRACKING_3D, RETRY};
	
	private GUIManagerListener listener;
	
	public Texture controlButtonTexture;
	public Texture tracking2DTexture;
	
	public GUIStyle custom;
	public GUIStyle textStyle;
	public GUIStyle clockStyle;
	public GUIStyle retryScreenStyle;
	
	
		
	private const String TIMER_TEXT = "Time: ";	

	private float displayTime;

	public float buttonSizeOriginal = 130;
	public float controlButtonSizeOriginal = 90;
	private float marginOriginal = 30;
		
	private float retryButtonSizeXOriginal = 80;
	private float retryButtonSizeYOriginal = 40;
	
	private float paddingRetryButtonYOriginal = 10;
	
	private float timerTextSizeXOriginal = 300;
	private float timerTextSizeYOriginal = 30;
	
	private float originalFontSize = 26;
	
	private float buttonSize;
	
	private float margin;
	private float marginPad;
	
	
	private float endTextSizeX;
	private float endTextSizeY;
		
	private float retryButtonSizeX;
	private float retryButtonSizeY;
	
	private float paddingRetryButtonY;
	
	private float timerTextSizeX;
	private float timerTextSizeY;
	
	private float baseWidth = Screen.width;
	private float baseHeight = Screen.height;
	
	private bool showTrackingControls = true;
	
	
	private float scaleFactor;
	
	public void setListener(GUIManagerListener listener){
		this.listener = listener;
	}	
	
	
	public void setDisplayTime(float displayTime) {
		this.displayTime = displayTime;
	}	
	
		
	
	public void setTrackingControls(bool showTrackingControls){
		this.showTrackingControls = showTrackingControls;
	}
	
	void onScale(float scaleFactor) {
        if (scaleFactor > 1.75) scaleFactor = 1.75f;
		this.scaleFactor = scaleFactor;
		
		buttonSize = buttonSizeOriginal * scaleFactor;
		
		margin = marginOriginal * scaleFactor;
	
			
		retryButtonSizeX = retryButtonSizeXOriginal * scaleFactor;
		retryButtonSizeY = retryButtonSizeYOriginal * scaleFactor;
	
		paddingRetryButtonY = paddingRetryButtonYOriginal * scaleFactor;
	
		timerTextSizeX = timerTextSizeXOriginal * scaleFactor;
		timerTextSizeY = timerTextSizeYOriginal * scaleFactor;	
		
		custom.fontSize = (int) (originalFontSize * scaleFactor);
		textStyle.fontSize = (int) (originalFontSize * scaleFactor);
		clockStyle.fontSize = (int) (originalFontSize * scaleFactor);
		retryScreenStyle.fontSize = (int) (originalFontSize * scaleFactor);
	}
	
	void OnGUI() {
		baseWidth = Screen.width;
		baseHeight = Screen.height;
		
		float xScale = (float) Screen.width / 800;
		float yScale = (float) Screen.height / 600;
		
		float scaleFactor = (xScale < yScale) ? xScale : yScale;
		
		if (this.scaleFactor != scaleFactor) onScale(scaleFactor);
		
		//help button leads to metaio web page
		if (GUI.Button(new Rect(baseWidth - (margin + buttonSize), margin, buttonSize, buttonSize), "?", custom)){
			listener.onButtonPressed(Buttons.HELP);		
		}
		
		//buttons to switch tracking configuration
		if (showTrackingControls){
			if (GUI.Button(new Rect(margin, margin, buttonSize, buttonSize), tracking2DTexture, GUI.skin.label)){
				listener.onButtonPressed(Buttons.TRACKING_2D);
			}
			if (GUI.Button(new Rect(margin, 2 * margin + buttonSize, buttonSize, buttonSize), "3D", custom)){
				listener.onButtonPressed(Buttons.TRACKING_3D);
			}
		}
		
			
    }
	
	Rect invert(Rect rect){
		return new Rect(rect.x, baseHeight - rect.y - rect.height, rect.width,rect.height);
	}	
	
	
}
