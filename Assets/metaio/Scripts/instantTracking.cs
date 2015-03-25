using UnityEngine;
using System.Collections;
using System;

public class instantTracking : MonoBehaviour 
{
	void Update () 
	{
		if (Input.touchCount > 0)
		{
			Touch t = Input.GetTouch(0);
			
			if (t.phase == TouchPhase.Ended)
			{
				Debug.Log("Starting instant tracking");
				
				String trackingConfiguration = "INSTANT_2D";
				
				// start instant tracking, the callback onInstantTrackingEvent will be
				// called once instant tracking is done.
				metaioSDK.startInstantTracking(trackingConfiguration, "");
			}
		}
	}
	

}

