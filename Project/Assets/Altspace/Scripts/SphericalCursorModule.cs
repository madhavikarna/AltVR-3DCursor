using UnityEngine;
using System;
using System.Collections;

public class SphericalCursorModule : MonoBehaviour {
	// This is a sensitivity parameter that should adjust how sensitive the mouse control is.
	public float Sensitivity;

	// This is a scale factor that determines how much to scale down the cursor based on its collision distance.
	public float DistanceScaleFactor;
	
	// This is the layer mask to use when performing the ray cast for the objects.
	// The furniture in the room is in layer 8, everything else is not.
	private const int ColliderMask = (1 << 8);

	// This is the Cursor game object. Your job is to update its transform on each frame.
	private GameObject Cursor;

	// This is the Cursor mesh. (The sphere.)
	private MeshRenderer CursorMeshRenderer;

	// This is the scale to set the cursor to if no ray hit is found.
	private Vector3 DefaultCursorScale = new Vector3(10.0f, 10.0f, 10.0f);

	// Maximum distance to ray cast.
	private const float MaxDistance = 100.0f;

	// Sphere radius to project cursor onto if no raycast hit.
	private const float SphereRadius = 1000.0f;

	private SphericalCoordinates sphericalCoordinates;


	void Awake() {
		// Referenced from http://wiki.unity3d.com/index.php/SphericalCoordinates
		sphericalCoordinates = new SphericalCoordinates(1000f, (Mathf.PI / 2f), 0f, 0f, 20f, 0f, -(Mathf.PI * 2f), -(Mathf.PI / 3f), (Mathf.PI / 3f));
		Cursor = transform.Find("Cursor").gameObject;
		CursorMeshRenderer = Cursor.transform.GetComponentInChildren<MeshRenderer>();
        CursorMeshRenderer.GetComponent<Renderer>().material.color = new Color(0.0f, 0.8f, 1.0f);
    }	


	void Update()
	{
		// TODO: Handle mouse movement to update cursor position.
		Vector2 mouseVector = Vector2.zero;
		mouseVector.x = -Input.GetAxis("Mouse X") * Sensitivity;
		mouseVector.y = Input.GetAxis("Mouse Y") * Sensitivity;
		sphericalCoordinates.SetRotation(sphericalCoordinates.polar + mouseVector.x, sphericalCoordinates.elevation + mouseVector.y);

		// TODO: Perform ray cast to find object cursor is pointing at.
		
		// TODO: Update cursor transform.

		var cursorHit = new RaycastHit();/* Your cursor hit code should set this properly. */;
		Vector3 exactPosition = gameObject.transform.position;
		Vector3 objectDirection = transform.TransformPoint(sphericalCoordinates.toCartesian) - exactPosition;
		Ray cameraRay = new Ray(exactPosition, objectDirection);

		RaycastHit finalTarget;
		if (Physics.Raycast(cameraRay, out finalTarget, 100f, 256))
		{
			Cursor.transform.position = finalTarget.point;
			float num = (finalTarget.distance * DistanceScaleFactor + 1f) / 2f;
			Cursor.transform.localScale = new Vector3(num, num, num);	
		}
		else
		{
			Cursor.transform.localPosition = sphericalCoordinates.toCartesian;
			Cursor.transform.localScale = DefaultCursorScale;
		}

		cursorHit = finalTarget;

		// Update highlighted object based upon the raycast.
		if (cursorHit.collider != null)
		{
			Selectable.CurrentSelection = cursorHit.collider.gameObject;
		}
		else
		{
			Selectable.CurrentSelection = null;
		}
	}
}
