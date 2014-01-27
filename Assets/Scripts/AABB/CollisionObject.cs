using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollisionObject : MonoBehaviour {

	public bool checkCollisionsEveryFrame = false;
	private Vector3 lastPosition;

	//This is where we will store our active collisions so that we're not sending messages every frame
	Dictionary<GameObject, bool> activeCollisions = new Dictionary<GameObject, bool>();
	
	void OnStart()
	{
		lastPosition = transform.position;
	}

	void OnEnable() {

		// Add to collision tracker list inside the Aabb static class when it's enabled/re-enabled.
		gameObject.TrackMyCollisions();

		CheckStatusOfCollisions();
	}

	void OnDisable() {

		// This will remove from collision tracker list inside the Aabb static class when the object is disabled.
		// When an object is destroyed, it disables itself first.

		Debug.LogWarning( "Untracking myself: " + gameObject.name );
		gameObject.UntrackMyCollisions();

	}
	
	public void CollisionTracker ( GameObject other )
	{
		//Check to see if this collision was already happening
		if ( !activeCollisions.ContainsKey( other ))
		{
			// Add this object to our list of active collisions if it's new.
			activeCollisions.Add ( other, true );

			// Let all the scripts do stuff based on this collision
			gameObject.SendMessage ( "CollisionEnter", other, SendMessageOptions.DontRequireReceiver );

		}

	}

	void LateUpdate() {

		//Cleans up the local active collision list at the end of the frame
		CheckStatusOfCollisions();

		//TEMPORARY LINE FOR TESTING PURPOSES ONLY
		if ( checkCollisionsEveryFrame && transform.position != lastPosition ) 
		{
			lastPosition = transform.position;
			gameObject.CollidingWithTrackedColliders();
		}

	}

	private void CheckStatusOfCollisions()
	{
		// Does a check on existing collisions. If one doesn't exist, take it out of the dictionary.
		if (activeCollisions.Count > 0)
		{
			Dictionary<GameObject, bool> activeCollisionsCopy = new Dictionary<GameObject, bool>(activeCollisions);

			foreach (GameObject o in activeCollisionsCopy.Keys)
			{
				if (!gameObject.CollidingWith( o ))
				{
					Debug.LogWarning( o.name + " is no longer colliding." );
					//if ( activeCollisions.ContainsKey( o ))
						activeCollisions.Remove( o );
				}
			}

		}

	}

	private void CollisionEnter ( GameObject other )
	{
		//This is what goes into your script

		Debug.LogWarning( gameObject.name + " collided with " + other.name + "." );
		//Debug.LogWarning( gameObject.name + " was at position " + gameObject.transform.position + ", and " + other.name + " was at position " + other.transform.position );
		Debug.LogWarning( gameObject.name + " was at position x:" + gameObject.transform.position.x + ", y:" + gameObject.transform.position.y + ", and " + other.name + " was at position " + other.transform.position );
	}
}
