using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Used to know if we are reloading from "Play Again"
    private static bool restartDirectToGame = false;

    [Header("UI Panels")]
    public GameObject startPanel;
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public GameObject pausePanel;
    public GameObject instructionsPanel;

    private bool isGameOver = false;
    private bool isPaused = false;
    private bool isInstructionsOpen = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Make sure these start hidden (we control them)
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (instructionsPanel != null) instructionsPanel.SetActive(false);

        if (restartDirectToGame)
        {
            // Coming from "Play Again" -> go straight into gameplay
            restartDirectToGame = false;

            if (startPanel != null)
                startPanel.SetActive(false);

            isGameOver = false;
            isPaused = false;
            isInstructionsOpen = false;
        }
        else
        {
            // First time launch -> show start menu
            if (startPanel != null)
                startPanel.SetActive(true);

            isGameOver = false;
            isPaused = true;         // pause while start menu is up
            isInstructionsOpen = false;
        }

        UpdatePauseState();
    }

    void Update()
    {
        if (isGameOver)
            return;

        // Pause toggle
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
                PauseGame();
            else
                ResumeGame();
        }

        // Instructions toggle (H key)
        if (Input.GetKeyDown(KeyCode.H))
        {
            ToggleInstructions();
        }
    }

    // ---------------- START GAME ----------------
    public void StartGame()
    {
        if (startPanel != null)
            startPanel.SetActive(false);

        isGameOver = false;
        isPaused = false;
        isInstructionsOpen = false;

        if (instructionsPanel != null) instructionsPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);

        UpdatePauseState();
    }

    // ---------------- DEATH / WIN ----------------
    public void OnPlayerDeath()
    {
        if (isGameOver) return;

        isGameOver = true;

        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        if (winPanel != null) winPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (instructionsPanel != null) instructionsPanel.SetActive(false);

        UpdatePauseState();
    }

    public void OnPlayerWin()
    {
        if (isGameOver) return;

        isGameOver = true;

        if (winPanel != null) winPanel.SetActive(true);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (instructionsPanel != null) instructionsPanel.SetActive(false);

        UpdatePauseState();
    }

    // ---------------- PAUSE ----------------
    public void PauseGame()
    {
        if (isGameOver) return;

        isPaused = true;
        if (pausePanel != null)
            pausePanel.SetActive(true);

        UpdatePauseState();
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (pausePanel != null)
            pausePanel.SetActive(false);

        UpdatePauseState();
    }

    // ---------------- INSTRUCTIONS ----------------
    public void ShowInstructions()
    {
        if (instructionsPanel == null || isGameOver)
            return;

        isInstructionsOpen = true;
        instructionsPanel.SetActive(true);
        UpdatePauseState();
    }

    public void HideInstructions()
    {
        if (instructionsPanel == null)
            return;

        isInstructionsOpen = false;
        instructionsPanel.SetActive(false);
        UpdatePauseState();
    }

    public void ToggleInstructions()
    {
        if (instructionsPanel == null)
            return;

        if (instructionsPanel.activeSelf)
            HideInstructions();
        else
            ShowInstructions();
    }

    // ---------------- BUTTONS ----------------
    public void RestartGame()
    {
        // Next scene load should skip start menu
        restartDirectToGame = true;

        // Reset time + flags before reload
        isGameOver = false;
        isPaused = false;
        isInstructionsOpen = false;
        Time.timeScale = 1f;

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

    // ---------------- HELPER ----------------
    private void UpdatePauseState()
    {
        bool startMenuActive = (startPanel != null && startPanel.activeSelf);

        bool shouldPause =
            isGameOver ||
            isPaused ||
            isInstructionsOpen ||
            startMenuActive;

        Time.timeScale = shouldPause ? 0f : 1f;
    }
}
