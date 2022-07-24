using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUIPanelDisplayer : MonoBehaviour
{
    public static GameUIPanelDisplayer Instance { get; private set; }

    [SerializeField] private GameObject panelStart;
    [SerializeField] private GameObject panelVictory;
    [SerializeField] private GameObject panelGameOver;

    [SerializeField] private ScoreController scoreController;
    [SerializeField] private SoundController soundController;

    private float timeScale = 1.5f;

    private void Awake()
    {
        // Check if there are any other instances conflicting
        if (Instance != null && Instance != this)
        {
            // If that is the case, destroy other instances
            Destroy(gameObject);
        }

        Instance = this;

        Time.timeScale = timeScale;

        scoreController.OnVictory += ShowVictoryPanel;
        scoreController.OnGameOver += ShowGameOverPanel;

        ShowStartPanel();
    }

    private void ShowStartPanel()
    {
        panelStart.SetActive(true);
    }

    public void HideStartPanel()
    {
        panelStart.SetActive(false);
    }
    
    private void ShowVictoryPanel()
    {
        panelVictory.SetActive(true);
        soundController.PlayGameComplete();
    }

    private void ShowGameOverPanel()
    {
        panelGameOver.SetActive(true);
        soundController.PlayGameComplete();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}