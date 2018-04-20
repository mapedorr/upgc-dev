﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MovingObject
{
	// ══════════════════════════════════════════════════════════════ PUBLICS ════
	// ═══════════════════════════════════════════════════════════ PROPERTIES ════
	float m_h;
	public float H { get { return m_h; } }

	float m_v;
	public float V { get { return m_v; } }

	bool m_inputEnabled;
	public bool InputEnabled { get { return m_inputEnabled; } set { m_inputEnabled = value; } }
	// ═════════════════════════════════════════════════════════════ PRIVATES ════

	// ══════════════════════════════════════════════════════════════ METHODS ════
	// override the Start function inherited from MovingObject
	protected override void Start ()
	{
		base.Start ();
	}

	// Update is called once per frame
	void Update ()
	{
		// if (!GameManager.instance.playersTurn) return;
		if (isMoving)
		{
			return;
		}

		if (m_inputEnabled)
		{
			m_h = Input.GetAxisRaw ("Horizontal");
			m_v = Input.GetAxisRaw ("Vertical");
		}
		else
		{
			m_h = 0f;
			m_v = 0f;
		}

		// this will prevent the Player to move diagonally
		if (m_h != 0)
		{
			m_v = 0;
		}

		if (m_h != 0 || m_v != 0)
		{
			// we expect to interact with a Wall ()
			AttemptMove<Box> (new Vector2 (m_h, m_v));
		}
	}

	protected override void OnCantMove<T> (T component)
	{
		Box box = component as Box;
		if (box != null)
		{
			if (box.Push (new Vector2 (m_h, m_v)))
			{
				ForceMove (new Vector2 (m_h, m_v), 0f);
			}
		}
	}
}