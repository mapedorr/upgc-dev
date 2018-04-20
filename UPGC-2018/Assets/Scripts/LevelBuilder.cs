using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelElement
{
	public string elementChar;
	public GameObject elementPrefab;
}

public class LevelBuilder : MonoBehaviour
{
	// ══════════════════════════════════════════════════════════════ PUBLICS ════
	public int currentLevel;
	public List<LevelElement> levelElements = new List<LevelElement> ();
	// ═══════════════════════════════════════════════════════════ PROPERTIES ════
	// ═════════════════════════════════════════════════════════════ PRIVATES ════
	BoardManager m_board;
	Levels m_levels;
	Level m_level;
	GameObject m_floorPrefab;

	// ══════════════════════════════════════════════════════════════ METHODS ════
	void Awake ()
	{
		m_levels = GetComponent<Levels> ();
		m_board = Object.FindObjectOfType<BoardManager> ();

		if (levelElements.Count > 0)
		{
			// get the prefab for the floor
			foreach (var element in levelElements)
			{
				Debug.Log (element.elementPrefab.name.Equals ("Floor"));
				if (element.elementPrefab.name.Equals ("Floor"))
				{
					m_floorPrefab = element.elementPrefab;
				}
			}
		}
	}

	GameObject GetPrefab (char c)
	{
		LevelElement levelElement = levelElements.Find (el => el.elementChar == c.ToString ());
		return (levelElement != null) ? levelElement.elementPrefab : null;
	}

	public void NextLevel ()
	{
		currentLevel++;
		if (currentLevel >= m_levels.LevelsList.Count)
		{
			currentLevel = 0;
		}
	}

	public void Build ()
	{
		if (m_board == null)
		{
			Debug.LogWarning ("LevelBuilder.Build ERROR: Not board found");
			return;
		}

		m_level = m_levels.LevelsList[currentLevel];
		// make the center of the level to be at 0,0
		int startX = -m_level.Width / 2;
		int x = startX;
		int y = -m_level.Height / 2;
		foreach (var row in m_level.Rows)
		{
			foreach (var character in row)
			{
				GameObject cellPrefab = GetPrefab (character);

				if (cellPrefab)
				{
					Instantiate (cellPrefab, new Vector2 (x, y), Quaternion.identity,
						(cellPrefab.GetComponent<Player> () == null) ? m_board.transform : null);
				}

				if (m_floorPrefab && (!cellPrefab ||
						(cellPrefab && cellPrefab.name != "Floor" && cellPrefab.name != "Wall")))
				{
					Instantiate (m_floorPrefab, new Vector2 (x, y), Quaternion.identity,
						m_board.transform);
				}

				x++;
			}
			y++;
			x = startX;
		}
	}
}