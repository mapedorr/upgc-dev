using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	// ══════════════════════════════════════════════════════════════ PUBLICS ════
	public static GameManager instance = null;
	// delay in between game stages
	public float delay = 1f;

	// events invoked for StartLevel/PlayLevel/EndLevel coroutines
	public UnityEvent setupEvent;
	public UnityEvent startLevelEvent;
	public UnityEvent playLevelEvent;
	public UnityEvent endLevelEvent;
	// ═══════════════════════════════════════════════════════════ PROPERTIES ════
	// has the user pressed start?
	bool m_hasLevelStarted = false;
	public bool HasLevelStarted { get { return m_hasLevelStarted; } set { m_hasLevelStarted = value; } }

	// have we begun gamePlay?
	bool m_isGamePlaying = false;
	public bool IsGamePlaying { get { return m_isGamePlaying; } set { m_isGamePlaying = value; } }

	// have we met the game over condition?
	bool m_isGameOver = false;
	public bool IsGameOver { get { return m_isGameOver; } set { m_isGameOver = value; } }

	// have the end level graphics finished playing?
	bool m_hasLevelFinished = false;
	public bool HasLevelFinished { get { return m_hasLevelFinished; } set { m_hasLevelFinished = value; } }
	// ═════════════════════════════════════════════════════════════ PRIVATES ════
	BoardManager m_boardManager;
	Player m_player;

	// ══════════════════════════════════════════════════════════════ METHODS ════
	void Awake ()
	{
		// apply Singleton pattern
		if (instance == null) instance = this;
		else if (instance != this) Destroy (gameObject);
		DontDestroyOnLoad (gameObject);

		// get components and dependencies
		m_boardManager = Object.FindObjectOfType<BoardManager> ().GetComponent<BoardManager> ();
	}

	// Use this for initialization
	void Start ()
	{
		// add null validations
		StartCoroutine ("RunGameLoop");
	}

	// Update is called once per frame
	void Update ()
	{

	}

	// run the main game loop, separated into different stages/coroutines
	IEnumerator RunGameLoop ()
	{
		yield return StartCoroutine ("StartLevelRoutine");
		yield return StartCoroutine ("PlayLevelRoutine");
		// yield return StartCoroutine ("EndLevelRoutine");
	}

	IEnumerator StartLevelRoutine ()
	{
		Debug.Log ("SETUP LEVEL");
		if (setupEvent != null)
		{
			setupEvent.Invoke ();
		}

		if (m_boardManager)
		{
			m_boardManager.SetupLevel ();
			m_player = Object.FindObjectOfType<Player> ().GetComponent<Player> ();
			if (m_player != null)
			{
				m_player.InputEnabled = true;
			}
		}

		yield return null;
	}

	IEnumerator PlayLevelRoutine ()
	{
		while (true)
		{
			yield return null;
		}
	}
}