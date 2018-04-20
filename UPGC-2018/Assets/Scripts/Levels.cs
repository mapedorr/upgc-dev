using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
	// ══════════════════════════════════════════════════════════════ PUBLICS ════
	// ═══════════════════════════════════════════════════════════ PROPERTIES ════
	List<string> m_rows = new List<string> ();
	public List<string> Rows { get { return m_rows; } }

	private int m_height;
	public int Height { get { return m_rows.Count; } }

	private int m_width;
	public int Width
	{
		get
		{
			int maxLength = 0;
			foreach (var row in m_rows)
			{
				// '\n' must be ignored
				if (row.Length - 1 > maxLength)
				{
					maxLength = row.Length - 1;
				}
			}
			return maxLength;
		}
	}
	// ═════════════════════════════════════════════════════════════ PRIVATES ════
	// ══════════════════════════════════════════════════════════════ METHODS ════
}

public class Levels : MonoBehaviour
{
	// ══════════════════════════════════════════════════════════════ PUBLICS ════
	public string filename;
	// ═══════════════════════════════════════════════════════════ PROPERTIES ════
	List<Level> m_levelsList = new List<Level> ();
	public List<Level> LevelsList { get { return m_levelsList; } }
	// ═════════════════════════════════════════════════════════════ PRIVATES ════
	char[] m_charsToTrim = { '\n', ' ' };
	// ══════════════════════════════════════════════════════════════ METHODS ════
	void Awake ()
	{
		TextAsset levelsTextAsset = (TextAsset) Resources.Load (filename);

		if (!levelsTextAsset)
		{
			Debug.Log ("Levels: " + filename + ".txt doesn't exist!");
			return;
		}

		string completeText = levelsTextAsset.text;
		string[] lines = completeText.Split (new string[] { "\n" }, System.StringSplitOptions.None);
		LevelsList.Add (new Level ());
		for (long i = 0; i < lines.LongLength; i++)
		{
			string line = lines[i];
			if (line.StartsWith (";"))
			{
				LevelsList.Add (new Level ());
				continue;
			}

			line = line.TrimEnd (m_charsToTrim);
			line = line.TrimEnd (System.Environment.NewLine.ToCharArray ());

			LevelsList[LevelsList.Count - 1].Rows.Add (line);
		}
	}
}