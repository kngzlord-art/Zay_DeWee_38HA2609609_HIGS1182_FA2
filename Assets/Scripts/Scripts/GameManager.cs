using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Start,
        Playing,
        GameOver,
        Win
    }

    public static GameManager Instance;

    [Header("Time Settings")]
    public TextMeshProUGUI timerDisplay;
    public float roundTime;
    public float time;

    [Header("Game State")]
    public GameState currentState;

    [Header("Score")]
    public int score = 0;
    public int totalItems = 5;

    [Header("Win Conditon")]
    public int totalAsteroids = 0;
    public int destroyedAsteroids = 0;

    [Header("UI")]
    public GameObject losePanel;
    public GameObject winPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI FuelPurchaseDisplay;
    public TextMeshProUGUI resultText;
    private TextMeshProUGUI highScoreText;

    public PlayerStats PS;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        time = roundTime;

        SetStartState();
        UpdateScoreUI();
    }

    private void Update()
    {
        if (PS != null)
        {
            FuelPurchaseDisplay.enabled = true;
            if (PS.Fuel == 0 && PS.Money >= 20)
            {
                if (Input.GetKeyDown(KeyCode.B))
                {
                    PS.loseMoney(20);
                    PS.addFuel(100);
                }
            }
            else if (PS.Fuel != 0)
            {
                FuelPurchaseDisplay.enabled = false;
            }

        }

        time -= Time.deltaTime;
        if (currentState == GameState.Playing)
        {
            if (time <= 0f)
            {
                GameOver();
            }
        }

        UpdateUI();

    }

    public void StartGame()
    {
        currentState = GameState.Playing;

        if (losePanel != null)
        {
            losePanel.SetActive(false);
        }

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        Debug.Log("Game Started");

        FuelPurchaseDisplay.enabled = false;
    }

    public void AddScore()
    {
        if (PS != null)
        {
            PS.addScore(1);
            score = PS.Score;
        }
        else
        {
            score++;
        }

        UpdateScoreUI();

        Debug.Log("Scrap collected. Score: " + score);
    }


    public void UpdateUI()
    {
        if (!timerDisplay)
        {
            return;
        }

        timerDisplay.text = Mathf.CeilToInt(time).ToString();
    }

    public void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Scrap: " + score;
        }
    }

    public void GameOver()
    {
        if (currentState == GameState.GameOver)
        {
            return;
        }

        currentState = GameState.GameOver;

        if (losePanel != null)
        {
            losePanel.SetActive(true);
        }

        ShowLose(time, PS.Score);

        Savehighscore();

        Debug.Log("Game Over");

        time = 0f;
    }

    public void WinGame()
    {

        if(currentState == GameState.Win || currentState == GameState.GameOver)
        {
            return;
        }
        

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        if(losePanel != null)
        {
            losePanel.SetActive(false);
        }


        ShowWin(time, PS.Score);

        Savehighscore();

        Debug.Log("Player collected all scrap. You Win!");
    }

    public void RestartGame()
    {
        Debug.Log("Restarting Game");

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMenu()
    {
        Debug.Log("Loading Menu");

        SceneManager.LoadScene("Main Menu");
    }

    private void SetStartState()
    {
        currentState = GameState.Start;


        if (losePanel != null)
        {
            losePanel.SetActive(false);
        }

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
    }

    public void ForceRefresh()
    {
        if (PS == null)
        {
            return;
        }

        score = PS.Score;

        UpdateScoreUI();

        if (PS.HealthSlider != null)
        {
            PS.HealthSlider.value = PS.Health;
        }

        if (PS.FuelSlider != null)
        {
            PS.FuelSlider.value = PS.Fuel;
        }

        if (PS.moneyDisplay != null)
        {
            PS.moneyDisplay.text = "Money: " + PS.Money;
        }

        if (PS.scoreDisplay != null)
        {
            PS.scoreDisplay.text = "Score: " + PS.Score;
        }
    }


    public void ShowLose(float time, int score)
    {
        string playerName = GetPlayerName();

        resultText.text = "The player " + playerName + " has died at " + Mathf.CeilToInt(time) + " seconds out and has obtained a score of " + score;
    }

    public void ShowWin(float time, int score)
    {
        string playerName = GetPlayerName();

        resultText.text = "The player " + playerName + " has won at " + Mathf.CeilToInt(time) + " seconds out and has obtained a score of " + score;
    }

    public void RegisterAsteroids()
    {
        totalAsteroids++;

        Debug.Log("The Asteroids have Spawned. The Total asteroids are " + totalAsteroids   );
    }

    public void AsteroidsDestroyed()
    {

        if(currentState == GameState.Win || currentState == GameState.GameOver)
        {
            return;
        }

        destroyedAsteroids++;

        Debug.Log("A spawned in Asteroid have been destroyed " + destroyedAsteroids + "/" + totalAsteroids);

        if(totalAsteroids > 0 && destroyedAsteroids >= totalAsteroids)
        {
            WinGame();
        }

    }


    public void Savehighscore()
    {
        int oldHighScore = PlayerPrefs.GetInt("HighScore", 0);

        int currentScore = PS.Score;

        if (currentScore > oldHighScore)
        {
            PlayerPrefs.SetInt("HighScore", currentScore);

            PlayerPrefs.Save();

            Debug.Log("New HighScore saved: " + currentScore);
        }
        else
        {
            Debug.Log("HighScore remains: " + oldHighScore);
        }
    }



    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GamePlay")
        {
            
            if(!timerDisplay)
            {
                var tagged = GameObject.FindGameObjectWithTag("Timer");
                if (tagged) timerDisplay = tagged.GetComponent<TextMeshProUGUI>();

                if(timerDisplay == null)
                {
                    GameObject byName = GameObject.Find("timerDisplay");
                    
                    if(!byName) timerDisplay = byName.GetComponent<TextMeshProUGUI>();
                }
            }

            UpdateUI();
            
        }
    }


    private string GetPlayerName()
        {
            if (PlayerDataManager.Instance != null)
            {
                if (!string.IsNullOrWhiteSpace(
                    PlayerDataManager.Instance.playerName))
                {
                    return PlayerDataManager.Instance.playerName;
                }
            }

            string savedName = PlayerPrefs.GetString("PlayerName", "");

            if (!string.IsNullOrWhiteSpace(savedName))
            {
                return savedName;
            }

            return "Unknown Player";
        }


}