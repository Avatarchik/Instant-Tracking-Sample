using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using metaio;
using metaio.common;

/// <summary>
/// This class provides main interface to the metaio SDK
/// </summary>
public class metaioSDK : MonoBehaviour
{
	// Ensure dependency DLLs can be loaded
	// (cf. http://forum.unity3d.com/threads/31083-DllNotFoundException-when-depend-on-another-dll)
	private void adjustPath()
	{
		var envPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
		var pluginsPath = Path.Combine(Path.Combine(Environment.CurrentDirectory, "Assets"), "Plugins");
		
#if UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR
		// Unfortunately we cannot use Application.dataPath in the loading thread (in which this constructor is called),
		// so we have to construct the path to "XYZ_Data/Plugins" ourself. Changing the PATH later (e.g. in Awake) does
		// not seem to work.

		// Search for "Plugins" folder in subfolders of the current directory (where the executable is)
		string[] subPaths = Directory.GetFileSystemEntries(Environment.CurrentDirectory);
		
		foreach (var subPath in subPaths)
		{
			var fullSubPath = Path.Combine(Environment.CurrentDirectory, subPath);
			// Only look at directories
			if (!Directory.Exists(fullSubPath))
				continue;
			
			// Use GetFullPath to ensure conversion of path separators (slash or backslash) to native
			var potentialPluginsPath = Path.GetFullPath(Path.Combine(fullSubPath, "Plugins"));
			if (Directory.Exists(potentialPluginsPath) && !envPath.Contains(Path.PathSeparator + potentialPluginsPath))
				envPath += Path.PathSeparator + potentialPluginsPath;
		}
#endif
		
		if ((pluginsPath.Length > 0 && !envPath.Contains(Path.PathSeparator + pluginsPath)))
			envPath += Path.PathSeparator + pluginsPath;
		
		Environment.SetEnvironmentVariable(
			"PATH",
			envPath,
			EnvironmentVariableTarget.Process);
	}
	
	public metaioSDK()
	{
		// Must be called before any calls to the metaio SDK DLL
		adjustPath();
	}
	
	
#region Public fields

	// Application signature for license verification
	public String applicationSignature;
	
	// Tracking configuration (path to file or a string)
	[SerializeField]
	public String trackingConfiguration;
	
	// Device camera index
	public static int cameraIndex = 0;
	
	// Device camera width
	public int cameraWidth = 320;
	
	// Device camera height
	public int cameraHeight = 240;

#endregion
	
#region Private fields
	
	// Defines whether the camera is active (as opposed to setImage)
	private static bool usingCamera = true;
	
#endregion
	
#region Editor script fields

	public static String[] trackingAssets = {"None", "DUMMY", "GPS", "ORIENTATION", "LLA", "CODE", "QRCODE", "StreamingAssets...", "Absolute Path...", "Generated"};
	
	[HideInInspector]
	[SerializeField]
	public int trackingAssetIndex;
	
	[HideInInspector]
	[SerializeField]
	public UnityEngine.Object trackingAsset = null;
	
#endregion
	
#region DLL functions
	
#if UNITY_IPHONE
	public const String METAIO_DLL = "__Internal";
#else
	public const String METAIO_DLL = "metaiosdk";
#endif
	
	/// <summary>
	/// Create the metaio SDK instance.
	/// </summary>
	/// <returns>
	/// 0 on sucess, non-zero on failure
	/// </returns>
	/// <param name='signature'>
	/// Application signature.
	/// </param>
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern int createMetaioSDKUnity(string signature);
	
	/// <summary>
	/// Delete the metaio SDK instance.
	/// </summary>
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern void deleteMetaioSDKUnity();
	
	/// <summary>
	/// Enable/disable metaio SDK callback.
	/// </summary>
	/// <param name='enable'>
	/// true to enable, false to disable
	/// </param>
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern void registerCallback(int enable);
	
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern int getUnityCallbackEventID();
	
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern IntPtr getUnityCallbackEventValue();
	
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern void removeUnityCallbackEvent();
	
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern void setScreenRotation(int rotation);
	
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern void resizeRenderer(int width, int height);
	
	// This is only here for platforms where GL.IssuePluginEvent seems not working (see below)
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern void UnityRenderEvent(int eventID);
		
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern int setTrackingConfiguration(string trackingConfiguration, int readFromFile);
	
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern int setCameraParametersInternal(string cameraFile);
	
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern void setRendererClippingPlaneLimits(float nearCP, float farCP);
	
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern uint getRequiredTextureSize();
	
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern float getCameraPlaneScale();

	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern void getSensorGravity(float[] values);
	
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern void getLocation(double[] values);
	
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern void getProjectionMatrix(float[] matrix);
	
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern float getTrackingValues(int cosID, float[] values);
	
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern float getTrackingFrameRate();

	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern void startSensors(int sensors);
	
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern void stopSensors(int sensors);
	
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern void pauseSensors();

	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	private static extern void setImageInternal(IntPtr buffer, uint width, uint height, int colorFormatInternal, uint originIsUpperLeft, int stride);
	
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	private static extern void setImageInternalFromImage(string filePath, out int outWidth, out int outHeight);

	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern void resumeSensors();
	
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern void setManualLocation(float latitude, float longitude, float altitude);
	
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern void resetManualLocation();
	
	/// <summary>
	/// Set the cos offset.
	/// </summary>
	/// <param name='coordinateSystemID'>
	/// Coordinate system ID
	/// </param>
	/// <param name='translation'>
	/// Translation (3 floats) or null to reset
	/// </param>
	/// <param name='rotation'>
	/// Rotation as quaternion (4 floats) or null to reset
	/// </param>
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern void setCosOffset(int coordinateSystemID, float[] translation, float[] rotation);
	
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern void requestCameraImage(string filepath, int width, int height);
	
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern int startInstantTracking(string trackingConfiguration, string outFile);
	
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern void convertLLAToTranslation(double latitude, double longitude, float[] translation);
		
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern IntPtr createGeometryFromMovie(string movieFilename);
	
	// Only used for movie textures currently
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern void deleteMovieGeometry(IntPtr movieGeometry);
	
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern uint getMovieTextureHeight(IntPtr movieGeometry);
	
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern uint getMovieTextureWidth(IntPtr movieGeometry);
	
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern float getMovieTextureDisplayAspect(IntPtr movieGeometry);
	
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern void setMovieTextureTargetTextureID(IntPtr movieGeometry, int textureID);
	
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern void startMovieTexture(IntPtr movieGeometry, int loop);
	
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern void pauseMovieTexture(IntPtr movieGeometry);

	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern void stopMovieTexture(IntPtr movieGeometry);
		
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern void requestVisualSearch(string databaseID, int returnFullTrackingConfig, string visualSearchServer);
	
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern IntPtr sensorCommandNative(string command, string parameter);
	
#if UNITY_IPHONE || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_EDITOR

	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern void startCameraInternal(int index, int width, int height);
	
	[DllImport(METAIO_DLL, CallingConvention=CallingConvention.Cdecl)]
	public static extern void stopCamera();
#elif UNITY_ANDROID
	/// <summary>
	///  Start device camera
	/// </summary>
	public static void startCameraInternal(int index, int width, int height)
	{
		
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
	
		Debug.Log("Application context: "+jo.ToString());
		
		AndroidJavaClass cls = new AndroidJavaClass("com.metaio.sdk.UnityProxy");
		object[] args = {jo, index, width, height};
		cls.CallStatic("StartCamera", args);
		
	}

	/// <summary>
	/// Stop device camera
	/// </summary>
	public static void stopCamera()
	{
		AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
		AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
	
		Debug.Log("Application context: "+jo.ToString());
		
		AndroidJavaClass cls = new AndroidJavaClass("com.metaio.sdk.UnityProxy");
		object[] args = {jo};
		cls.CallStatic("StopCamera", args);
	}
	
	
#endif
	
#endregion
	
	public static bool setCameraParameters(string cameraFile)
	{
		int res = setCameraParametersInternal(cameraFile);
		metaioCamera.updateCameraProjectionMatrix();
		return res != 0;
	}
	
	public static void startCamera(int index, int width, int height)
	{
		usingCamera = true;
		startCameraInternal(index, width, height);
		metaioCamera.updateCameraProjectionMatrix();
	}
	
	/// <summary>
	/// Updates the screen orientation of metaio SDK
	/// </summary>
	/// <param name='orientation'>
	/// Screen orientation.
	/// </param>
	public static void updateScreenOrientation(ScreenOrientation orientation)
	{
		switch (orientation)
		{
		case ScreenOrientation.LandscapeLeft:
			setScreenRotation(0);
			break;
		case ScreenOrientation.LandscapeRight:
			setScreenRotation(2);
			break;
		case ScreenOrientation.Portrait:
			setScreenRotation(3);
			break;
		case ScreenOrientation.PortraitUpsideDown:
			setScreenRotation(1);
			break;
		}
	}
	
	/// <summary>
	/// Set an image file as image source
	/// </summary>
	/// <remarks>
	/// This method is used to set the image source from a file for rendering and tracking. It will automatically stop
	/// camera capture if currently running. Call startCamera again to resume capturing from camera.
	/// </remarks>
	/// <param name='filePath'>
	/// Path to the image file
	/// </param>
	/// <returns>
	/// Resolution of the image if loaded successfully, else a null vector
	/// </returns>
	public static Vector2di setImage(string filePath)
	{
		int width = 0;
		int height = 0;
		
		usingCamera = false;
		setImageInternalFromImage(filePath, out width, out height);
		
		if (width > 0 && height > 0)
		{
			uint largerDimension = (uint)Math.Max(width, height);
			uint powerOf2 = 1;
			while (powerOf2 < largerDimension)
				powerOf2 *= 2;

			// Make sure camera texture is resized in case we need a larger one
			metaioDeviceCamera deviceCamera = (metaioDeviceCamera)FindObjectOfType(typeof(metaioDeviceCamera));
			if (deviceCamera != null)
				deviceCamera.prepareForPotentialTextureSizeChange(powerOf2);
		}

		metaioCamera.updateCameraProjectionMatrix();

		return new Vector2di(width, height);
	}
	
	public static void setImage(byte[] buffer, uint width, uint height, ColorFormat colorFormat, bool originIsUpperLeft)
	{
		// Cannot use default parameters in Unity, forces .NET 3.5 syntax
		setImage(buffer, width, height, colorFormat, originIsUpperLeft, -1);
	}

	public static void setImage(byte[] buffer, uint width, uint height, ColorFormat colorFormat, bool originIsUpperLeft, int stride)
	{
		GCHandle arrayHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
		IntPtr ptr = arrayHandle.AddrOfPinnedObject();
		
		uint largerDimension = Math.Max(width, height);
		uint powerOf2 = 1;
		while (powerOf2 < largerDimension)
			powerOf2 *= 2;

		// Make sure camera texture is resized in case we need a larger one
		metaioDeviceCamera deviceCamera = (metaioDeviceCamera)FindObjectOfType(typeof(metaioDeviceCamera));
		if (deviceCamera != null)
			deviceCamera.prepareForPotentialTextureSizeChange(powerOf2);

		usingCamera = false;
		setImageInternal(ptr, width, height, (int)colorFormat, originIsUpperLeft ? 1U : 0U, stride);

		arrayHandle.Free();

		metaioCamera.updateCameraProjectionMatrix();
	}
	
	/// <summary>
	/// Send sensor command
	/// </summary>
	/// <returns>
	/// Result of the sensor command
	/// </returns>
	/// <param name='command'>
	/// Command to send
	/// </param>
	/// <param name='parameter'>
	/// Parameter to send
	/// </param>
	public static String sensorCommand(string command, string parameter)
	{
		IntPtr resultPtr = metaioSDK.sensorCommandNative(command, parameter);
		string result = Marshal.PtrToStringAnsi(resultPtr);
		return result;
	}
	
	/// <summary>
	/// Sets the tracking configuration from resource or a named string.
	/// </summary>
	/// <returns>
	/// non-zero in case of success, else 0
	/// </returns>
	/// <param name='trackingConfig'>
	/// XML file name in the resource, or a named string, e.g. "LLA" or "QRCODE"
	/// </param>
	public static int setTrackingConfigurationFromAssets(string trackingConfig)
    {
        int result = 0;
		
		// first check inside streaming assets
		String assetPath = AssetsManager.getAssetPath(trackingConfig);
			
		if (assetPath != null)
		{
			result = metaioSDK.setTrackingConfiguration(assetPath, 1);
		}
		else if (trackingConfig != null)
		{
			Debug.Log("Tracking configuration '" +trackingConfig + "' not found in the streaming assets, loading it as absolute path or string");
			result = metaioSDK.setTrackingConfiguration(trackingConfig, 1);
		}
		
		return result;
    }
	
	void Awake()
	{
		if (!SystemInfo.graphicsDeviceVersion.ToLowerInvariant().Contains("opengl"))
			Debug.LogError("#######################\n" +
			               "It seems that another renderer than OpenGL is used, but OpenGL is required when using " +
			               "the metaio SDK. Please pass \"-force-opengl\" to the executable to enforce running " +
			               "with OpenGL.\n" +
			               "#######################");

		
		AssetsManager.extractAssets(true);	
	}
	
	void Start () 
	{
		int result = createMetaioSDKUnity(applicationSignature);
		if (result == 0)
			Debug.Log("metaio SDK created successfully");
		else
			Debug.LogError("Failed to create metaio SDK!");

		updateScreenOrientation(Screen.orientation);
				
		// Start the camera
		startCamera(cameraIndex, cameraWidth, cameraHeight);
		
		// Load tracking configuration
		result = setTrackingConfigurationFromAssets(trackingConfiguration);
				
		if (result == 0)
			Debug.LogError("Start: failed to load tracking configuration: "+trackingConfiguration);
		else
			Debug.Log("Loaded tracking configuration: "+trackingConfiguration);
		
	}
	
	void OnDisable()
	{
		Debug.Log("OnDisable: deleting metaio sdk...");
		
		// stop camera before deleting the instance
		stopCamera();
		
		// delete the instance
		deleteMetaioSDKUnity();
	}
	
	void OnApplicationPause(bool pause)
	{
		Debug.Log("OnApplicationPause: "+pause);
		
		if (pause)
		{
			pauseSensors();
			
			if (usingCamera)
				stopCamera();
		}
		else
		{
			resumeSensors();
			
			if (usingCamera)
				startCamera(cameraIndex, cameraWidth, cameraHeight);
		}
	}
	
}
