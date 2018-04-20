using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
	// ══════════════════════════════════════════════════════════════ PUBLICS ════
	// the layer in which the collisions will be checked
	public LayerMask blockingLayer;
	// where the player is currently headed 
	public Vector3 destination;
	// is the player currently moving?
	public bool isMoving = false;
	// what easetype to use for iTweening
	public iTween.EaseType easeType = iTween.EaseType.easeInSine;
	// how fast we move
	public float moveSpeed = 3f;
	// delay to use before any call to iTween
	public float iTweenDelay = 0f;
	// ═══════════════════════════════════════════════════════════ PROPERTIES ════
	// ═════════════════════════════════════════════════════════════ PRIVATES ════
	BoxCollider2D m_boxCollider;
	// reference to the RigidBody of the object that is being moved

	// ══════════════════════════════════════════════════════════════ METHODS ════
	// protected function can be overwriten by inheriting classes
	protected virtual void Start ()
	{
		m_boxCollider = GetComponent<BoxCollider2D> ();
	}

	protected virtual bool AttemptMove<T> (Vector2 direction, float delayTime = 0f) where T : Component
	{
		RaycastHit2D hit;
		bool canMove = Move (direction, out hit, delayTime);

		if (hit.transform == null)
		{
			return true;
		}

		T hitComponent = hit.transform.GetComponent<T> ();

		if (!canMove && hitComponent != null)
		{
			OnCantMove (hitComponent);
		}

		return canMove;
	}

	// function that will return a boolean and a RaycastHit2D
	// out >> causes arguments be passed by reference, so a modification on the parameter will modify
	//        also the original bar passed to the function where was called
	protected bool Move (Vector2 direction, out RaycastHit2D hit, float delayTime)
	{
		Vector2 start = transform.position;
		Vector2 end = start + direction;

		// assure that the casted rays will not hit the collider of the object that is going to be moved
		m_boxCollider.enabled = false;

		// cast a line from the start point to the end point checking collisions in the blockingLayer
		hit = Physics2D.Linecast (start, end, blockingLayer);

		m_boxCollider.enabled = true;

		if (hit.transform == null)
		{
			// init the tween that will move the GameObject
			StartCoroutine (MoveRoutine (end, delayTime));
			return true;
		}

		// the GameObject can't move
		return false;
	}

	protected void ForceMove (Vector2 direction, float delayTime = 0.25f)
	{
		Vector2 start = transform.position;
		Vector2 end = start + direction;

		StartCoroutine (MoveRoutine (end, delayTime));
	}

	// coroutine used to move the player
	IEnumerator MoveRoutine (Vector2 destinationPos, float delayTime)
	{
		// we are moving
		isMoving = true;

		// set the destination to the destinationPos being passed into the coroutine
		destination = destinationPos;

		// pause the coroutine for a brief periof
		yield return new WaitForSeconds (delayTime);

		// move the player toward the destinationPos using the easeType and moveSpeed variables
		iTween.MoveTo (gameObject, iTween.Hash (
			"x", destinationPos.x,
			"y", destinationPos.y,
			"delay", iTweenDelay,
			"easetype", easeType,
			"speed", moveSpeed
		));

		while (Vector2.Distance (destinationPos, transform.position) > float.Epsilon)
		{
			yield return null;
		}

		// stop the iTween immediately
		iTween.Stop (gameObject);

		// set the player position to the destination explicitly
		transform.position = destinationPos;

		// we are not moving
		isMoving = false;
	}

	protected abstract void OnCantMove<T> (T component) where T : Component;

}