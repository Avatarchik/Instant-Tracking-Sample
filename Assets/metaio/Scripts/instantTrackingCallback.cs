using UnityEngine;
using System.Collections;
using System;

public class instantTrackingCallback : metaioCallback
{
	override protected void onInstantTrackingEvent(String filepath)
	{
		Debug.Log("onInstantTrackingEvent: "+filepath);
		
		// if succeeded, set new tracking configuration
		if (filepath.Length > 0)
		{
			int result = metaioSDK.setTrackingConfiguration(filepath, 1);
			Debug.Log("onInstantTrackingEvent: instant tracking configuration loaded: "+result);
		}
		
	}
}
