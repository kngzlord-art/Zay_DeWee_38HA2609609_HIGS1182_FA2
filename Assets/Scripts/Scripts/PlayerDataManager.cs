using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;

    [Header("Name")]
    public TMP_InputField nameInput;
    public GameObject continuebutton;

    [Header("scene")]
    public string gameplaySceneName = "GamePlay";

    public string playerName;

    private void Awake()
    {
        if
            (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);

            playerName = PlayerPrefs.GetString("PlayerName");
        }
        else
        {
            Destroy(gameObject);
        }

       

    }

    private void Start()
    { 
        nameInput.text = playerName;

        CheckName();
    }

    public void CheckName()
    {
        if(string.IsNullOrWhiteSpace(nameInput.text))
        {
            continuebutton.SetActive(false);
        }
        else
        {
            continuebutton.SetActive(true);
        }

    }


    public void ContinueGame()
    {
        if(string.IsNullOrWhiteSpace(nameInput.text))
        {
            return;
        }

        playerName = nameInput.text;

        PlayerPrefs.SetString("PlayerName", playerName);

        PlayerPrefs.Save();

        SceneManager.LoadScene(gameplaySceneName);
    }

}
