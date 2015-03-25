using UnityEngine;
using UnityEditor;
using System;
using System.IO;

[CustomEditor(typeof(metaioSDK))]
public class metaioSDKEditor : Editor {
	
	// referece to the metaioSKD
	private metaioSDK metaioSDK;
	
	
	
	public void OnEnable()
    {
        metaioSDK = (metaioSDK)target;
		
    }
	
	void OnGUI ()
	{
		GUILayout.Label ("metaio SDK", EditorStyles.boldLabel);
	}
	

	public override void OnInspectorGUI()
	{
		//base.OnInspectorGUI();
		
		//if (GUI.changed)
		//	EditorUtility.SetDirty(target);
			
		// info text
		EditorGUILayout.HelpBox("The metaioSDK compnent will be used to configure the tracking, preview the camera, " +
		 	"tranfrom the main camera and provide a valid SDK license. If you use the Unity build-in configuratio, " +
		 	"please use read the documenation at http://dev.metaio.com/sdk", MessageType.Info);
		
		// signature - maybe display the original signature
		metaioSDK.applicationSignature = EditorGUILayout.TextField("SDK Signature", metaioSDK.applicationSignature);
		metaioSDK.cameraWidth = EditorGUILayout.IntField("Camera width", metaioSDK.cameraWidth);
        metaioSDK.cameraHeight = EditorGUILayout.IntField("Camera height", metaioSDK.cameraHeight);
        metaioSDK.trackingAssetIndex = EditorGUILayout.Popup("Select configuration", metaioSDK.trackingAssetIndex, metaioSDK.trackingAssets, EditorStyles.popup);
	
	
		if (metaioSDK.trackingAssetIndex == 7)
		{
			// select from streaming assets
			metaioSDK.trackingConfiguration="tracking.xml";
			EditorGUILayout.HelpBox("Just drag&drop a *.xml, *.3dmap or *.zip file with tracking data from your project view here", MessageType.Info);
			metaioSDK.trackingAsset = EditorGUILayout.ObjectField( metaioSDK.trackingAsset, typeof(UnityEngine.Object), true);
			
			// set the actual file path
			metaioSDK.trackingConfiguration = AssetDatabase.GetAssetPath(metaioSDK.trackingAsset);
			metaioSDK.trackingConfiguration = metaioSDK.trackingConfiguration.Replace("Assets/StreamingAssets/", "");
			//Debug.Log("Tracking configuration dragged: " + metaioSDK.trackingConfiguration);
		}
		else if (metaioSDK.trackingAssetIndex == 8)
		{
			// specify absolute path
			metaioSDK.trackingConfiguration = EditorGUILayout.TextField("Tracking Configuration", metaioSDK.trackingConfiguration);
		}
		else if (metaioSDK.trackingAssetIndex == 9)
		{
			// generate tracking xml
			metaioSDK.trackingConfiguration="TrackingConfigGenerated.xml";
		}
		else if (metaioSDK.trackingAssetIndex > 0)
		{
			metaioSDK.trackingConfiguration = metaioSDK.trackingAssets[metaioSDK.trackingAssetIndex];
		}
		else
		{
			metaioSDK.trackingConfiguration = "";
			Debug.LogWarning("No tracking configuration selected");
		}
		
	
		
		/*if(metaioSDK.useCustomXML)
		{
			GUILayout.Label("Reference to the .xml (must be in StreamingAsset folder)");
			metaioSDK.textAsset = (TextAsset) EditorGUILayout.ObjectField("XML configuration", metaioSDK.textAsset, typeof(TextAsset), false);

			if(metaioSDK.textAsset)
				metaioSDK.trackingConfiguration = metaioSDK.textAsset.name;
			else
				Debug.LogError("You need to specify a valid XML configuration when in 'Custom XML' mode");
		}*/
		
		// here we can add more options
		if (GUI.changed) 
			EditorUtility.SetDirty(target);
	}
}
