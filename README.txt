WHAT IT DOES================================================
This performs simple Aabb collision checks on movement or whenever you want. Simply by 
adding a script to game objects you want involved in the system, it will add them to a 
tracker list and appropriately remove them from said list when they are 
destroyed/disabled. Right now, I have no trees to reduce checks by region, but it has 
some handy tools, and is built to allow you to plug it into your own system, whatever
that may be. In addition to the standard collision checks that are used in the
automation, you can check collisions between two specific objects ( Colliding ), 
or feed it your own list of objects to check against 
(gameObject.isCollidingWithDictionaryObjects).

WHY THIS EXISTS=============================================
The Unity3D team has put a lot of effort into giving better tools to those who make 2D 
games. Unfortunately, they make the assumption that everyone wants to move everything 
using their physics engine. Some genres like fighting games, and many lessons in 
classic game design require manual movement control, and physics simulation bugs can 
be really tough to resolve. After looking all over, I couldn't find any 2D collision 
solution. I and several others were frustrated with this issue, so I decided to make 
something we could use. I'm a new programmer, so by making this public, I figure others 
can help me steer this thing right.

LICENSE=====================================================
I want everyone to be able to use this in their commercial projects, since I want to help 
people, and I believe this will result in more people wanting to contribute. I went with 
the MIT License as described in LICENSE.txt.

HOW TO USE==================================================
Attach the CollisionObject.cs script to any object you'd like to track. It requires a 
BoxCollider2D component, and from there it should be automatic. When one tracked object 
hits another, it will send an AabbCollisionEnter message to both objects involved in the 
collision. Check the CollisionObject script for more info on that. You can also check the 
box for exit messages if you'd like those.

For simplicity's sake, I have it automatically check for collisions every frame on 
movement. If you'd like more control over when things are checked you could disable the
automation and put these checks in a character controller script instead. Also by default 
in these early stages, I have it log when a collision occurs in the CollisionObject 
script, but that whole AabbCollisionEnter method can be removed from CollisionObject.cs.
It's just an example, and I figure people will want to do some tests at this place in 
development.

FEATURES====================================================
- Automated tracking system done by simply adding the CollisionObject script to an object
- Announce collisions to tracked objects, and detect them with the AabbCollisionEnter
       method.
- Check collisions at a test position. Handy in character controllers.
- Check collision against a supplied list of objects so you can hook it into your own
       tree, test against only ground tiles, etc...

TO DO======================================================
- Add support for rotated collision boxes. Right now it only supports boxes with zero
       degrees rotation.
- Make it update collision checks on scale change. Right now it's just position change.
- Add ability to check every possible collision withiin a list, instead of just from a
       single moving object's perspective.
- Maybe use the collision layer system built into Unity to ignore collisions with objects
       in certain layers, or only check against a specific layer.
