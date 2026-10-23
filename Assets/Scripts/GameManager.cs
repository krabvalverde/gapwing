using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum State { Menu, Ready, Playing, Paused, GameOver }

    public static GameManager Instance { get; private set; }
    static bool skipMenu;

    [SerializeField] float baseSpeed = 3f, speedStep = 0.1f, maxSpeed = 6f;
    [SerializeField] float baseGap = 4.5f, gapStep = 0.05f, minGap = 3f;
    [SerializeField] GameObject menuPanel, readyPanel, hudPanel, pausePanel, gameOverPanel;
    [SerializeField] TMP_Text scoreText, finalScoreText, bestScoreText;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip flapClip, scoreClip, hitClip;

    public State Current { get; private set; }
    public int Score { get; private set; }
    public float Speed => Current == State.GameOver ? 0f : Mathf.Min(baseSpeed + Score * speedStep, maxSpeed);
    public float Gap => Mathf.Max(baseGap - Score * gapStep, minGap);

    void Awake()
    {
        Instance = this;
        SetState(skipMenu ? State.Ready : State.Menu);
        skipMenu = false;
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame && (Current == State.Playing || Current == State.Paused))
            TogglePause();
    }

    void SetState(State state)
    {
        Current = state;
        Time.timeScale = state == State.Paused ? 0f : 1f;
        menuPanel.SetActive(state == State.Menu);
        readyPanel.SetActive(state == State.Ready);
        hudPanel.SetActive(state == State.Ready || state == State.Playing || state == State.Paused);
        pausePanel.SetActive(state == State.Paused);
        gameOverPanel.SetActive(state == State.GameOver);
    }

    public void Play() => SetState(State.Ready);

    public void TogglePause() => SetState(Current == State.Paused ? State.Playing : State.Paused);

    public void Restart()
    {
        skipMenu = true;
        ToMenu();
    }

    public void ToMenu() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void OnFlap()
    {
        if (Current == State.Ready) SetState(State.Playing);
        PlaySound(flapClip);
    }

    public void AddScore()
    {
        if (Current != State.Playing) return;
        Score++;
        scoreText.text = Score.ToString();
        PlaySound(scoreClip);
    }

    public void GameOver()
    {
        if (Current != State.Playing) return;
        PlaySound(hitClip);
        int best = Mathf.Max(Score, PlayerPrefs.GetInt("Best"));
        PlayerPrefs.SetInt("Best", best);
        PlayerPrefs.Save();
        finalScoreText.text = $"Pontos: {Score}";
        bestScoreText.text = $"Recorde: {best}";
        SetState(State.GameOver);
    }

    void PlaySound(AudioClip clip)
    {
        if (clip) audioSource.PlayOneShot(clip);
    }
}
