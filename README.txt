- Note that box colliders are used.
- Colliders, scripts and tags attached to flamable objects are attached to cubes in the models.
- Objects have weird origin positions - rotating makes the rotation like on a large circle, fire starts somewhere else (far away from the actual object).
- Changed how the steam effect works. Not a coroutine anymore. Instantiated and it destroys itself after 1.2s (particle duration option)
- Ammunition for water gun works properly now.
- Cleaned the code.

- Added house health bar and attached a script to empty game object "HouseHealthObject"
- Added ThrowObjectScript and Rigidbody to objects that are small - currently to armchair, coffeetable, Chair, Chair (1), Chair (2), Chair (3), TV


