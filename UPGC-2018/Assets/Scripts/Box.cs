using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MovingObject
{
	// override the Start function inherited from MovingObject
	protected override void Start ()
	{
		base.Start ();
	}

	protected override void OnCantMove<T> (T component)
	{
		// TODO: implement OnCantMove behaviour
	}

	public bool Push (Vector2 direction)
	{
		return AttemptMove<Wall> (direction);
	}
}