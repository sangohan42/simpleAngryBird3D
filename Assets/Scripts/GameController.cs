using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public static GameController Instance { get; private set; }

	public GameObject panelStart;
	public GameObject panelVictory;
	public GameObject panelGameOver;

	public bool isPlaying;
	public enum State {InGame, Paused, Complete, StartUp}
	public State gameState;

	public UnityEvent OnGameStart;
	public UnityEvent OnGameComplete;
	private float timeScale = 1.5f;
	
	void Awake () 
	{
        // Check if there are any other instances conflicting
        if (Instance != null && Instance != this)
        {
            // If that is the case, destroy other instances
            Destroy(gameObject);
        }

        Instance = this;

        Time.timeScale = timeScale;
		gameState = State.StartUp;

		ShowStartPanel();
	}

	void ShowStartPanel()
	{ 
		panelStart.SetActive(true); 
	}

	public void HideStartPanel()
	{
		if(panelStart.activeInHierarchy)
		{
			panelStart.SetActive(false);
		}
	}

	public void StartPlay()
	{
		isPlaying = true;
		gameState = State.InGame;

		OnGameStart.Invoke();
	}
	
	void Complete()
	{
		isPlaying = false;
		gameState = State.Complete;
		SoundController.Instance.playGameOver();

		OnGameComplete.Invoke();
	}

	public void Victory()
	{
		if (gameState != State.Complete) 
		{
			Complete();
			panelVictory.SetActive(true);
		}
	}

	public void GameOver()
	{
		if (gameState != State.Complete) 
		{
			Complete();
			panelGameOver.SetActive(true);
		}
	}
	
	public void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
