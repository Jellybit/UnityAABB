using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (BoxCollider2D))] 
public class CollisionObject : MonoBehaviour {

	//Usually, I let my character controller handle the collision checks, so that it can react, but I have this set
	//to true by default because I assume people will want the easiest way first.
	public bool autoCheckForCollisionsOnMovement = true;
	public bool collisionExitMessages = false;


	private Vector3 lastPosition;

	//This is where we will store our active collisions so that we're not sending messages every frame
	Dictionary<GameObject, bool> activeCollisions = new Dictionary<GameObject, bool>();
	
	void OnStart()
	{
		// This is storing your position so that it knows when this object has moved and needs to recheck.
		// We check collisions on start so that we know if we have collisions on the first frame before it moves.
		lastPosition = transform.position;

		if ( autoCheckForCollisionsOnMovement ) 
		{
			gameObject.announceCollidingWithTrackedColliders();
		}
	}

	void OnEnable() {

		// Add to collision tracker list inside the Aabb static class when it's enabled/re-enabled.
		gameObject.TrackMyCollisions();

		CheckStatusOfCollisions();
	}

	void OnDisable() {

		// This will remove from collision tracker list inside the Aabb static class when the object is disabled.
		// When an object is destroyed, it disables itself first.
		gameObject.UntrackMyCollisions();

	}

	//This method is called from the Aabb script on successful collision involving this object.
	public void CollisionTracker ( GameObject other )
	{
		//Check to see if this collision was already happening
		if ( !activeCollisions.ContainsKey( other ))
		{
			// Add this object to our list of active collisions if it's new.
			activeCollisions.Add ( other, true );
			
			// Let all the scripts do stuff based on this collision
			gameObject.SendMessage ( "AabbCollisionEnter", other, SendMessageOptions.DontRequireReceiver );
			
		}
		
	}

	void LateUpdate() {

		//Cleans up the local active collision list at the end of the frame
		CheckStatusOfCollisions();

		// This is for checking collisions automatically every frame, if you've marked that as true.
		if ( autoCheckForCollisionsOnMovement && transform.position != lastPosition ) 
		{
			lastPosition = transform.position;
			gameObject.announceCollidingWithTrackedColliders();
		}

	}

	// Does a check on existing collisions. If one is no longer active, take it out of the active collisions list.
	private void CheckStatusOfCollisions()
	{
		if (activeCollisions.Count > 0)
		{
			Dictionary<GameObject, bool> activeCollisionsCopy = new Dictionary<GameObject, bool>(activeCollisions);

			foreach (GameObject o in activeCollisionsCopy.Keys)
			{
				if (!gameObject.isCollidingWith( o ))
				{
					if ( activeCollisions.ContainsKey( o ))
						activeCollisions.Remove( o );

					if ( collisionExitMessages )
						gameObject.SendMessage ( "AabbCollisionExit", o, SendMessageOptions.DontRequireReceiver );
				}
			}

		}

	}

	//This example method goes into the other scripts that need to know when a collision has happened.
	//You may erase this method from this script if you wish.
	private void AabbCollisionEnter ( GameObject other )
	{
		//I added this line because I assume you want to test collisions right out the box. This isn't required.
		Debug.Log( "(" + Time.time + "): " + gameObject.name + " collided with " + other.name + "." );
	}
}