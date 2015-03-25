using UnityEngine;
using System.Collections;
using System;

public class readQRCodes : metaioCallback 
{
	/// <summary>
	/// This is called when a QR code is detected or lost
	/// </summary>
	/// <param name='trackingEvent'>
	/// QR code data, or empty when lost
	/// </param>
	override protected void onTrackingEvent(String trackingEvent)
	{
		Debug.Log("onTrackingEvent: "+trackingEvent);
		
		guiText.text = trackingEvent;
		
	}
}
