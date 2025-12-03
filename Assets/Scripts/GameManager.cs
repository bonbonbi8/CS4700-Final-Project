using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public GameObject gameOverPanel; // use your existing one
    public GameObject winPanel;      // new (or can reuse a panel if you already have)
    public GameObject pausePanel;    // use your existing one

    private bool isGameOver = false;
    private bool isPaused = false;

    void Awake()
    {
        // Simple singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
    }

    void Update()
    {
        if (isGameOver)
            return;

        // Pause toggle
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                ShowPause();
            else
                ResumeFromPause();
        }
    }

    // Called by PlayerStats.OnDeath
    public void OnPlayerDeath()
    {
        if (isGameOver) return;

        isGameOver = true;
        Time.timeScale = 0f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    // Called by WinTrigger when player wins
    public void OnPlayerWin()
    {
        if (isGameOver) return;

        isGameOver = true;
        Time.timeScale = 0f;

        if (winPanel != null)
            winPanel.SetActive(true);
    }

    // UI BUTTONS -------------------

    public void RestartGame()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        isPaused = false;

        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ShowPause()
    {
        if (isGameOver) return;

        isPaused = true;
        Time.timeScale = 0f;
        if (pausePanel != null)
            pausePanel.SetActive(true);
    }

    public void ResumeFromPause()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }
}
