WHAT IT DOES================================================
This performs simple Aabb collision checks on movement or whenever you want. Simply by 
adding a script to components you want involved in the system, it will add them to a 
tracker list and appropriately remove them from said list when they are destroyed/disabled. 
Right now, I have no trees to reduce checks by region, but it has some handy tools. You 
can check collisions between two specific objects ( Colliding( gameObject1, gameObject2 )), 
or feed it your own list of objects to check against 
(gameObject.CollidingWithObjectsInDictionary( dictionary )).

HISTORY=====================================================
The Unity3D team has put a lot of effort into giving better tools to those who make 2D 
games. Unfortunately, they make the assumption that everyone wants to move everything using 
their physics engine. Some genres like fighting games, and many lessons in classic game 
design require manual movement control, and physics simulation bugs can be really tough to
resolve. After looking all over, I couldn't find any 2D collision solution. I and several 
people I knew were frustrated with this issue, so I decided to make something we could use. 
I'm a new programmer, so by making this public, I figure others can help me steer this 
thing right.

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
movement. I personally like more control over when things are checked and put these checks 
in my character controller script instead. Also by default in these early stages, I have 
it log when a collision occurs in the CollisionObject script, but that whole 
AabbCollisionEnter method can be removed. I just figure people will want to do some tests 
at this place in development.

FUTURE======================================================
I'd like to flesh out something like a quadtree/r-tree/somethingtree to keep collision 
checks down, but I was having some difficulty getting that done right now. Also, there are 
different approaches I'd take between a platformer and a bullet hell shooter, so I'd like 
to maybe figure out a graceful way to switch between modes. Please feel free to 
contribute. I want this to be for everyone.
