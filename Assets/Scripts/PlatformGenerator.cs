using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformGenerator : MonoBehaviour 
{
	public List<GameObject> Platforms = new List<GameObject>();

	public int Level = 0;
	public int LevelInterval = 5;

	public Vector2 OffsetX = Vector2.zero;
	public Vector2 OffsetY = Vector2.zero;

	private int _platformCounter = 0;
	private GameObject _lastPlatform;

	private Transform _transform;

	void Awake()
	{
		_transform = transform;


		for (int i = 0; i < 15; i++) 
		{
			GenerateNewPlatform ();	

		}

	}

	private void GenerateNewPlatform()
	{

		float randomXOffset = Random.Range (OffsetX.x, OffsetX.y);
		float newYposition = Random.Range (OffsetY.x, OffsetY.y);
		if (_lastPlatform) 
		{
			newYposition += _lastPlatform.transform.position.y;
		}

		Vector3 newPosition = new Vector3  (_transform.position.x + randomXOffset, 
											newYposition, 
											_transform.position.z);

		GameObject newPlatform = Platforms [Level];

		GameObject platformClone = Instantiate (newPlatform, newPosition, Quaternion.identity);
		platformClone.transform.SetParent (_transform);

		platformClone.AddComponent<ObjectMover> ();
		ObjectMover objectMover = platformClone.GetComponent<ObjectMover> ();

		objectMover.ObjectToMove = platformClone.transform;

		Transform Waypoint1 = platformClone.transform.Find ("Waypoint1").GetComponent<Transform> ();
		Transform Waypoint2 = platformClone.transform.Find ("Waypoint2").GetComponent<Transform> ();
		Waypoint1.parent = null;
		Waypoint2.parent = null;
		objectMover.Waipoints.Add (Waypoint1);
		objectMover.Waipoints.Add (Waypoint2);
        objectMover.WaitAtWaipoint = Random.Range(objectMover.WaitAtWaipoint, objectMover.WaitAtWaipoint + 1);

		_lastPlatform = platformClone;

		_platformCounter++;

		if (_platformCounter % LevelInterval == 0)
			Level++;
	}

}
