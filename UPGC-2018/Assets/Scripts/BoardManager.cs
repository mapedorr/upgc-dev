using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

	// ══════════════════════════════════════════════════════════════ PUBLICS ════
	// uniform distance between nodes
	public static float spacing = 1f;

	// four compass directions
	public static readonly Vector2[] directions = {
		new Vector2 (spacing, 0f),
		new Vector2 (-spacing, 0f),
		new Vector2 (0f, spacing),
		new Vector2 (0f, -spacing)
	};

	// ═══════════════════════════════════════════════════════════ PROPERTIES ════

	// ═════════════════════════════════════════════════════════════ PRIVATES ════
	LevelBuilder m_levelBuilder;
	Player m_player;

	// ══════════════════════════════════════════════════════════════ METHODS ════
	// Awake is called when the script instance is being loaded.
	void Awake ()
	{
		m_levelBuilder = Object.FindObjectOfType<LevelBuilder> ().GetComponent<LevelBuilder> ();
	}

	public void SetupLevel ()
	{
		if (m_levelBuilder)
		{
			m_levelBuilder.Build ();
		}
	}
}