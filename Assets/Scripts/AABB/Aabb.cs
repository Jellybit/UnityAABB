using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public static class Aabb
	
{

	public static Dictionary<GameObject, Rect> trackedColliders = new Dictionary<GameObject, Rect>();
	
	// Use this one on your transform like so: transform.Colliding( otherGameObject );
	public static bool CollidingWith( this GameObject self, GameObject other )
	{


		if ( !self.collider2D.enabled || !other.collider2D.enabled )
			return false;

		Rect aBox = self.BoxToRect();
		Rect bBox = other.BoxToRect();

		// Find out if these guys intersect
		return Intersect ( aBox, bBox );
	}

	/*

	// Use this one on your transform like so: transform.Colliding( otherGameObject );
	public static bool CollidingWithList( this Transform self, List<GameObject> others )
	{
		
		bool colliding = false;
		Rect myCollider = self.gameObject.BoxToRect();

		colliding = self.RectCollidingWithList( others, myCollider );

		// Find out if these guys intersect
		return colliding;
	}

	public static bool RectCollidingWithList( this Transform self, List<GameObject> others, Rect testCollider )
	{
		
		bool colliding = false;
		
		foreach (GameObject other in others)
		{
			if ( self != other )
			{
				if (Intersect( testCollider, other.BoxToRect()))
				{
					colliding = true;
					
					SendCollisionInfo( self.gameObject, other );
				}
				
				
			}
			
		}
		
		// Find out if these guys intersect
		return colliding;
	}

	public static bool CollidingWithTrackedColliders( this Transform self )
	{
		
		bool colliding = false;
		Rect myCollider = self.gameObject.BoxToRect();
		
		colliding = self.RectCollidingWithTrackedColliders( myCollider );
		
		// Find out if these guys intersect
		return colliding;
	}

	*/

	public static bool CollidingWithTrackedColliders( this GameObject self )
	{

		self.UpdateMyCollisions();
		return self.RectCollidingWithObjectsInDictionary( trackedColliders[self], trackedColliders );

	}

	public static bool RectCollidingWithTrackedColliders( this GameObject self, Rect testCollider )
	{

		return self.RectCollidingWithObjectsInDictionary( testCollider, trackedColliders );
		
	}

	public static bool RectCollidingWithObjectsInDictionary( this GameObject self, Rect testCollider, Dictionary<GameObject, Rect> colliderDictionary )
	{
		
		bool colliding = false;

		Dictionary<GameObject, Rect> colliderDictionaryCopy = new Dictionary<GameObject, Rect>(colliderDictionary);
		
		foreach (var pair in colliderDictionaryCopy)
		{
			if ( self != pair.Key )
			{
				if (Intersect( testCollider, pair.Value ))
				{
					colliding = true;
					
					SendCollisionInfo( self, pair.Key );
				}
				
				
			}
			//else
				//Debug.LogWarning( "Skipping self: " + self.name );
			
		}
		
		// Find out if these guys intersect
		return colliding;
	}

	public static bool Colliding( GameObject a, GameObject b )
	{
		if ( !a.collider2D.enabled || !b.collider2D.enabled )
			return false;

		Rect aBox = a.BoxToRect();
		Rect bBox = b.BoxToRect();

		// Find out if these guys intersect
		return Intersect ( aBox, bBox );
	}
	
	// This checks the intersection between two rectangles.
	public static bool Intersect( Rect a, Rect b ) 
	{
		// Basic AABB collision detection. All of these must be true for there to be a collision.
		bool comp1 = a.yMin > b.yMax;
		bool comp2 = a.yMax < b.yMin;
		bool comp3 = a.xMin < b.xMax;
		bool comp4 = a.xMax > b.xMin;

		//Debug.Log ( "(" + System.DateTime.Now + ") 1: " +comp1+ ", 2: " +comp2+ ", 3: " +comp3+ ", 4: " +comp4 );

		// This will only return true if all are true.
		return comp1 && comp2 && comp3 && comp4;
	}

	// This converts a BoxCollider2D to a rectangle for use in determining an intersection
	public static Rect BoxToRect ( this GameObject a )
	{
		// Finding the BoxCollider2D for the GameObject.
		BoxCollider2D aCollider = a.GetComponent<BoxCollider2D> ();
		
		// Grabbing the GameObject position, converting it to Vector2.
		Vector2 aPos = a.transform.position.V2();
		
		// Now that we have the object's worldspace location, we offset that to the BoxCollider's center
		aPos += aCollider.center;
		// From the center, we find the top/left corner by cutting the total height/width in half and 
		// offset by that much
		float sizeX = aCollider.size.x * a.transform.localScale.x;
		float sizeY = aCollider.size.y * a.transform.localScale.y;

		aPos.x -= Mathf.Abs( sizeX )/2;
		aPos.y += Mathf.Abs( sizeY )/2;
		
		// Create a rectangle based on the top/left corner, and extending the rectangle 
		// to the width/height
		return new Rect ( aPos.x, aPos.y, sizeX, -sizeY );
	}
	
	// Converts a Vector 3 to Vector 2
	public static Vector2 V2 ( this Vector3 v ) 
	{
		// Converts the Vector3 to a Vector2.
		return new Vector2 (v.x, v.y);
	}

	public static Dictionary<GameObject, Rect> ListToCollisionDict( List<GameObject> list )
	{
		Dictionary<GameObject, Rect> dictionary = new Dictionary<GameObject, Rect>();

		foreach (GameObject item in list )
		{
			dictionary.Add( item, item.BoxToRect() );
		}

		return dictionary;
	}

	// Sends collision information to both objects involved in the collision
	private static void SendCollisionInfo ( GameObject object1, GameObject object2 ) 
	{
		
		object1.NotifyOfCollisionWith( object2 );
		object2.NotifyOfCollisionWith( object1 );
		
		
	}
	
	// This finds the Collision Object script and tells it a collision was found
	private static void NotifyOfCollisionWith ( this GameObject self, GameObject other ) 
	{
		
		CollisionObject collisionScript = self.GetComponent<CollisionObject>();
		
		collisionScript.CollisionTracker( other );
		
	}

	//Adds object to the list of tracked collisions.
	public static void TrackMyCollisions ( this GameObject self )
	{
		trackedColliders.Add ( self, self.BoxToRect() );
	}

	public static void UntrackMyCollisions ( this GameObject self )
	{
		trackedColliders.Remove ( self );
	}

	public static void UpdateMyCollisions ( this GameObject self )
	{
		trackedColliders[self] = self.BoxToRect();
	}

	//This version lets you specify a rectangle apart from where you exist
	public static void UpdateMyCollisions ( this GameObject self, Rect testRectangle )
	{
		trackedColliders[self] = testRectangle;
	}
	
}