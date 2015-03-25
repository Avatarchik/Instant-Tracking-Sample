using UnityEngine;
using System;


public class metaioTracker : MonoBehaviour 
{
#region Public fields
	// per default, we tranform the camera instead of the objects
	// Note: if you need to tranform multiple objects, the camera should be fixed (e.g. set to false)
	public bool tranformCamera = true;
	
	// link to the main camera
	public Camera cameraToPosition;

	// COS ID
	[SerializeField]
	public int cosID = 1;	
	
	// the texture, needed for editor
	[SerializeField]
	public Texture2D texture;

	[SerializeField]
	public String trackingImage;
	
	// true if we want to use a preconfigure pose
	public bool simulatePose = false;
#endregion	

	// Holds temprary tracking values
	private float[] trackingValues;	
	
	// true if childs' rendering is enabled, else false
	private bool childsEnabled;
		
	void Awake() 
	{
		trackingValues = new float[7];	
		
		// find the MainCamera, it should have the metaioCamera script attached
		cameraToPosition = Camera.main;
		if(tranformCamera && ! cameraToPosition)
		{
			 Debug.LogError("there is no cameraToPosition set");
		}
		
		childsEnabled = true;
	}

	// Update is called once per frame
	void Update  () 
	{
		if(simulatePose)
		{
			// use this predefines pose
			Quaternion q = new Quaternion(-0.2f, -0.8f, 0.4f, 0.4f);
			Vector3 p = new Vector3(14.7f, 12.8f, 203.0f);
			if(tranformCamera){
				setCameraPosition(p,q);
			}else{
				transform.position = p;
				transform.rotation = q;
			}
			return;
		}
		
		float quality = metaioSDK.getTrackingValues(cosID, trackingValues);
		//Debug.Log("cosID " + cosID + " quality: " + quality );
		
		if (quality > 0f)
		{
			Quaternion q;
			q.x = trackingValues[3];
			q.y = trackingValues[4];
			q.z = trackingValues[5];
			q.w = trackingValues[6];

			//translation
			Vector3 p;
			p.x = trackingValues[0];
			p.y = trackingValues[1];
			p.z = trackingValues[2];
			
			if(tranformCamera){
				setCameraPosition(p,q);
			}else{
				transform.position = p;
				transform.rotation = q;
			}
				
			// show childs
			enableRenderingChilds(true);
		}
		else
		{
			// hide because target not tracked
			enableRenderingChilds(false);
		}
	}

	
	private void setCameraPosition( Vector3 p, Quaternion q)
	{
		// todo, make a function out of this, otherwhise its the same as metaioTracker.cs
		Matrix4x4 rotationMatrix = new Matrix4x4();
		NormalizeQuaternion(ref q);

		rotationMatrix.SetTRS(Vector3.zero, 
		                       q,
		                       new Vector3(1.0f, 1.0f, 1.0f));

		Matrix4x4 translationMatrix = new Matrix4x4();
		translationMatrix.SetTRS(p, 
		                       new Quaternion(0.0f, 0.0f, 0.0f, 1.0f),
		                       new Vector3(1.0f, 1.0f, 1.0f));
		
		//split up rotation and translation
		Matrix4x4 composed = translationMatrix * rotationMatrix;
		//from world to camera so we have to invert the matrix
		Matrix4x4 invertedMatrix = composed.inverse;

        //center the camera in front of goal - z-axis			
		cameraToPosition.transform.position = invertedMatrix.GetColumn(3);
		cameraToPosition.transform.rotation = QuaternionFromMatrix(invertedMatrix);
	}
	

	private void TransformFromMatrix(Matrix4x4 matrix, Transform trans) {
	    trans.rotation = QuaternionFromMatrix(matrix);
	    trans.position = matrix.GetColumn(3); // uses implicit conversion from Vector4 to Vector3
	}



	private Quaternion QuaternionFromMatrix(Matrix4x4 m) {
	    // Adapted from: http://www.euclideanspace.com/maths/geometry/rotations/conversions/matrixToQuaternion/index.htm

	    Quaternion q = new Quaternion();
	    q.w = Mathf.Sqrt( Mathf.Max( 0, 1 + m[0,0] + m[1,1] + m[2,2] ) ) / 2; 
	    q.x = Mathf.Sqrt( Mathf.Max( 0, 1 + m[0,0] - m[1,1] - m[2,2] ) ) / 2; 
	    q.y = Mathf.Sqrt( Mathf.Max( 0, 1 - m[0,0] + m[1,1] - m[2,2] ) ) / 2; 
	    q.z = Mathf.Sqrt( Mathf.Max( 0, 1 - m[0,0] - m[1,1] + m[2,2] ) ) / 2; 

	    q.x *= Mathf.Sign( q.x * ( m[2,1] - m[1,2] ) );
	    q.y *= Mathf.Sign( q.y * ( m[0,2] - m[2,0] ) );
	    q.z *= Mathf.Sign( q.z * ( m[1,0] - m[0,1] ) );

	    return q;

	}


	private void NormalizeQuaternion (ref Quaternion q)
	{
	    float sum = 0;
	    for (int i = 0; i < 4; ++i)
	        sum += q[i] * q[i];
	    float magnitudeInverse = 1 / Mathf.Sqrt(sum);
	    for (int i = 0; i < 4; ++i)
	        q[i] *= magnitudeInverse;
	}
	
	// Enable/disable rendering
	private void enableRenderingChilds(bool enable)
    {
		// Do nothing if enabled state is not changed
		if (childsEnabled == enable)
			return;
		
        Renderer[] rendererComponents = GetComponentsInChildren<Renderer>();

        foreach (Renderer component in rendererComponents) 
		{
            component.enabled = enable;
        }
		
		childsEnabled = enable;
		
    }
	
	public void ApplyModifications()
	{
		// thenn set the material
		// renderer.material.SetTexture( "_MainTex", texture ) ;
		Transform previewTransform = transform.FindChild("PreviewPlane");
		if(previewTransform==null)
			return;
		
		GameObject previewPlane = previewTransform.gameObject;
		
		// set the texture preview
		previewPlane.renderer.sharedMaterial.SetTexture( "_MainTex", texture ) ;
		
		// scale the plane
		previewPlane.transform.localScale = new Vector3( 20f*texture.texelSize.x*texture.width, 1, 20f*texture.texelSize.x*texture.width); 
		Quaternion rotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
		rotation.eulerAngles = new Vector3(0, -90, 0);
		previewPlane.transform.localRotation = rotation;
		
	}
	
	
}

