using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public static class Aabb
	
{

	public static Dictionary<GameObject, Rect> trackedColliders = new Dictionary<GameObject, Rect>();
	
	// This checks the intersection between two rectangles. It is used by all the other collision methods.
	public static bool Intersecting( Rect a, Rect b ) 
	{
		// Basic AABB collision detection. All of these must be true for there to be a collision.
		bool comp1 = a.yMin > b.yMax;
		bool comp2 = a.yMax < b.yMin;
		bool comp3 = a.xMin < b.xMax;
		bool comp4 = a.xMax > b.xMin;
		
		// This will only return true if all are true.
		return comp1 && comp2 && comp3 && comp4;
	}

	// Use this one on your game object like so: gameObject.Colliding( otherGameObject );
	public static bool isCollidingWith( this GameObject self, GameObject other )
	{


		if ( !self.collider2D.enabled || !other.collider2D.enabled )
			return false;

		Rect aBox = self.BoxToRect();
		Rect bBox = other.BoxToRect();

		// Find out if these guys intersect
		return Intersecting ( aBox, bBox );
	}

	// Same thing, different approach
	public static bool Colliding( GameObject a, GameObject b )
	{
		return a.isCollidingWith( b );
	}


	public static bool isCollidingWithTrackedColliders( this GameObject self )
	{

		self.UpdateMyCollisions();
		return self.RectCollidingWithDictionaryObjects( trackedColliders[self], trackedColliders );

	}

	public static bool isCollidingWithDictionaryObjects( this GameObject self, Dictionary<GameObject, Rect> colliderDictionary )
	{
		return self.RectCollidingWithDictionaryObjects( self.BoxToRect(), colliderDictionary );
	}

	public static bool RectCollidingWithTrackedColliders( this GameObject self, Rect testCollider )
	{

		return self.RectCollidingWithDictionaryObjects( testCollider, trackedColliders );
		
	}

	public static bool RectCollidingWithDictionaryObjects( this GameObject self, Rect testCollider, Dictionary<GameObject, Rect> colliderDictionary )
	{
		
		bool colliding = false;

		Dictionary<GameObject, Rect> colliderDictionaryCopy = new Dictionary<GameObject, Rect>(colliderDictionary);
		
		foreach (var pair in colliderDictionaryCopy)
		{
			if ( self != pair.Key )
			{
				if (Intersecting( testCollider, pair.Value ))
				{
					colliding = true;
					
					AnnounceCollision( self, pair.Key );
				}
				
				
			}
			
		}

		return colliding;
	}

	// This converts a BoxCollider2D to a rectangle for use in determining an intersection
	public static Rect BoxToRectAtPosition ( this GameObject a, Vector3 testPositionV3 )
	{
		// Finding the BoxCollider2D for the GameObject.
		BoxCollider2D boxCollider = a.GetComponent<BoxCollider2D> ();

		// Converting to vector 2 so we can add another vector 2 to it.
		Vector2 testPosition = testPositionV3.V2();

		// Using a given worldspace location, we offset that to the BoxCollider's center
		testPosition += boxCollider.center;
		// From the center, we find the top/left corner by cutting the total height/width in half and 
		// offset by that much
		float rectWidth = boxCollider.size.x * a.transform.localScale.x;
		float rectHeight = boxCollider.size.y * a.transform.localScale.y;
		
		testPosition.x -= Mathf.Abs( rectWidth )/2;
		testPosition.y += Mathf.Abs( rectHeight )/2;
		
		// Create a rectangle based on the top/left corner, and extending the rectangle 
		// to the width/height
		return new Rect ( testPosition.x, testPosition.y, rectWidth, -rectHeight );
	}

	// This converts a BoxCollider2D to a rectangle using the object's actual position
	public static Rect BoxToRect ( this GameObject self )
	{
		return self.BoxToRectAtPosition( self.transform.position );
	}
	
	// Converts a Vector 3 to Vector 2
	public static Vector2 V2 ( this Vector3 v ) 
	{
		// Converts the Vector3 to a Vector2.
		return new Vector2 (v.x, v.y);
	}

	// Converts a list of objects to a dictionary.
	public static Dictionary<GameObject, Rect> ListToCollisionDict( this List<GameObject> list )
	{
		Dictionary<GameObject, Rect> dictionary = new Dictionary<GameObject, Rect>();

		foreach (GameObject item in list )
		{
			dictionary.Add( item, item.BoxToRect() );
		}

		return dictionary;
	}

	// Sends collision information to both objects involved in the collision
	private static void AnnounceCollision ( GameObject object1, GameObject object2 ) 
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

	public static Dictionary<GameObject, Rect> UpdateAllCollisionsInThisDictionary( this Dictionary<GameObject, Rect> colliderDictionary )
	{
		
		Dictionary<GameObject, Rect> colliderDictionaryCopy = new Dictionary<GameObject, Rect>(colliderDictionary);
		
		foreach (var pair in colliderDictionaryCopy)
		{
			colliderDictionaryCopy[pair.Key] = pair.Key.BoxToRect();			
		}
		
		return colliderDictionaryCopy;
	}

	public static void UpdateAllTrackedCollisions( )
	{
		trackedColliders = trackedColliders.UpdateAllCollisionsInThisDictionary();
	}
	
}

// check point for collision
// check for collision without announcing
