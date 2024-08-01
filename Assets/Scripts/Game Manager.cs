using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI introText;
    public TextMeshProUGUI instructionsText;
    public TextMeshProUGUI controlsText;
    public TextMeshProUGUI gameTimerText;
    public TextMeshProUGUI fallTimerText;
    public TextMeshProUGUI remainingCubesText;

    public float introDuration = 3f;
    public float instructionsDuration = 5f;
    public float controlsDuration = 5f; 
    public float gameDuration = 120f;
    public float maxFallTime = 5f;

    private float gameTimeRemaining;
    public bool isGameTimerRunning = false;
    private float fallTime = 0f;

    private Player player;
    private int totalCubes = 5;
    private int collectedCubes = 0;

    private void Start()
    {
        // Initial setup
        introText.text = "Collect all the objects before time runs out!";
        instructionsText.text = "Face where the red arrow is facing to change gravity.";
        controlsText.text = "Use WASD or Arrow keys to move. Press Space to jump.\n and use Arrow key to change gravity to the top use Cntrl + UpArrow";

        gameTimerText.gameObject.SetActive(false);
        fallTimerText.gameObject.SetActive(false);
        remainingCubesText.gameObject.SetActive(true);
        instructionsText.gameObject.SetActive(false);
        controlsText.gameObject.SetActive(false);

        player = FindObjectOfType<Player>();

        if (player == null)
        {
            Debug.LogError("Player object not found in the scene!");
            return;
        }

        UpdateRemainingCubesText();
        StartCoroutine(StartGameSequence());
    }

    private IEnumerator StartGameSequence()
    {
        // Show intro text
        introText.gameObject.SetActive(true);
        yield return new WaitForSeconds(introDuration);
        introText.gameObject.SetActive(false);

        // Show instructions text
        instructionsText.gameObject.SetActive(true);
        yield return new WaitForSeconds(instructionsDuration);
        instructionsText.gameObject.SetActive(false);

        // Show controls text
        controlsText.gameObject.SetActive(true);
        yield return new WaitForSeconds(controlsDuration);
        controlsText.gameObject.SetActive(false);

        // Start game timer
        gameTimerText.gameObject.SetActive(true);
        gameTimeRemaining = gameDuration;
        isGameTimerRunning = true;

        // Allow player movement
        if (player != null)
        {
            player.EnableMovement(true);
        }
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
        if (Application.CanStreamedLevelBeLoaded(2))
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            Debug.LogError("Scene 2 could not be loaded.");
        }
    }

    public void GameOver()
    {
        if (Application.CanStreamedLevelBeLoaded(1))
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.LogError("Scene 1 could not be loaded.");
        }
    }
}
