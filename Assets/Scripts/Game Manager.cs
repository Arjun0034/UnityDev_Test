using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public TextMeshProUGUI introText;
    public TextMeshProUGUI gameTimerText;
    public TextMeshProUGUI fallTimerText;
    public TextMeshProUGUI remainingCubesText;

    public float introDuration = 5f;
    public float gameDuration = 120f;
    public float maxFallTime = 5f;

    private float gameTimeRemaining;
    public bool isGameTimerRunning = false;
    private bool isFalling = false;
    private float fallTime = 0f;

    private Player player;
    private int totalCubes = 5;
    private int collectedCubes = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        introText.text = "Collect all the objects before time runs out!";
        gameTimerText.gameObject.SetActive(false);
        fallTimerText.gameObject.SetActive(false);
        remainingCubesText.gameObject.SetActive(true);

        player = FindObjectOfType<Player>();
        UpdateRemainingCubesText();
        StartCoroutine(StartGameTimerAfterIntro());
    }

    private IEnumerator StartGameTimerAfterIntro()
    {
        yield return new WaitForSeconds(introDuration);

        introText.gameObject.SetActive(false);
        gameTimerText.gameObject.SetActive(true);

        gameTimeRemaining = gameDuration;
        isGameTimerRunning = true;
    }

    private void Update()
    {
        if (isGameTimerRunning)
        {
            gameTimeRemaining -= Time.deltaTime;

            if (gameTimeRemaining <= 0)
            {
                gameTimeRemaining = 0;
                isGameTimerRunning = false;
                CheckGameOver();
            }

            UpdateGameTimerText(gameTimeRemaining);
        }

        if (player != null)
        {
            if (!player.IsGrounded())
            {
                fallTime += Time.deltaTime;
                UpdateFallTimer(fallTime);
                if (fallTime >= maxFallTime)
                {
                    CheckGameOver();
                }
            }
            else
            {
                fallTime = 0f;
                isFalling = false;
                fallTimerText.gameObject.SetActive(false);
            }
        }
    }

    private void UpdateGameTimerText(float timeRemaining)
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        gameTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void UpdateFallTimer(float time)
    {
        fallTimerText.gameObject.SetActive(true);
        fallTimerText.text = "Fall Timer: " + time.ToString("F2") + "s";
    }

    private void UpdateRemainingCubesText()
    {
        remainingCubesText.text = "Cubes: " + (totalCubes - collectedCubes);
    }

    public void CollectCube()
    {
        collectedCubes++;
        UpdateRemainingCubesText();

        if (collectedCubes >= totalCubes)
        {
            WinGame();
        }
    }

    private void CheckGameOver()
    {
        if (collectedCubes < totalCubes)
        {
            GameOver();
        }
    }

    private void WinGame()
    {
        SceneManager.LoadScene(2);
    }

    private void GameOver()
    {
        SceneManager.LoadScene(1);
    }

   
}
