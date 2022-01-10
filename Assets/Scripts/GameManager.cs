using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float PlayersBloodLeft = 100f;
    public int PlayersTotalGoatsEaten;
    private Vector2 _playersDefaultPos = new Vector2(0f, 0f);

    private Vector2 _enemy1sDefaultPos = new Vector2(32.75f, 0f);
    private Vector2 _enemy2sDefaultPos = new Vector2(32.75f, 6f);

    public bool WeekIsSettingUp;
    public int Week;
    private WeekManager _weekManagerScript;

    public static GameManager s_Instance = null;

    private GameObject _weekMenuBackgroundImage;
    private Text _weekLoseText;
    private Button _startButton;
    private Button _restartButton;
    private Button _quitButton;

    private void Awake()
    {
        if (s_Instance == null)
        {
            s_Instance = this;
        }
        else if (s_Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        PlayersTotalGoatsEaten = 0;

        WeekIsSettingUp = true;
        Week = 0;
        _weekManagerScript = gameObject.GetComponent<WeekManager>();
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Week++;
        WeekMenu();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void WeekMenu()
    {
        WeekIsSettingUp = true;

        _weekMenuBackgroundImage = GameObject.Find("WeekMenuBackgroundImage");
        _weekMenuBackgroundImage.SetActive(true);

        _weekLoseText = GameObject.Find("WeekLoseText").GetComponent<Text>();
        _weekLoseText.text = "WEEK " + Week.ToString();

        _startButton = GameObject.Find("StartButton").GetComponent<Button>();
        _startButton.onClick.AddListener(StartNextWeekOnClick);
        _startButton.gameObject.SetActive(true);

        _restartButton = GameObject.Find("RestartButton").GetComponent<Button>();
        _restartButton.onClick.AddListener(RestartOnClick);
        _restartButton.gameObject.SetActive(true);

        _quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
        _quitButton.onClick.AddListener(QuitOnClick);
        _quitButton.gameObject.SetActive(true);

        //if (Week == 1)
        //{
        //    ScreenCapture.CaptureScreenshot("D:/Repos/ChupacabraGame/Screenshots/WeekMenu.png");
        //}
    }

    private void StartNextWeekOnClick()
    {
        _weekManagerScript.GenerateFor(Week);

        _startButton.gameObject.SetActive(false);
        _restartButton.gameObject.SetActive(false);
        _quitButton.gameObject.SetActive(false);

        _weekLoseText.text = "";

        _weekMenuBackgroundImage.SetActive(false);

        WeekIsSettingUp = false;
    }

    private void RestartOnClick()
    {
        PlayersBloodLeft = 100f;
        PlayersTotalGoatsEaten = 0;
        Week = 0;

        ResetPlayer();
        ResetEnemies();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ResetPlayer()
    {
        GameObject player = GameObject.Find("Player");
        player.transform.position = _playersDefaultPos;

        PlayerController playerControllerScript = player.GetComponent<PlayerController>();
        playerControllerScript.BloodLeft = PlayersBloodLeft;
        playerControllerScript.TotalGoatsEaten = PlayersTotalGoatsEaten;
        playerControllerScript.CurrentWeeksGoatCount = Week % 10 + 1;
    }

    private void ResetEnemies()
    {
        GameObject enemy1 = GameObject.Find("Enemy1");
        enemy1.transform.position = _enemy1sDefaultPos;

        GameObject enemy2 = GameObject.Find("Enemy2");
        enemy2.transform.position = _enemy2sDefaultPos;
    }

    private void QuitOnClick()
    {
        Application.Quit();
    }

    public void GameOverMenu()
    {
        WeekIsSettingUp = true;

        _weekMenuBackgroundImage.SetActive(true);

        _weekLoseText.text = "YOU MADE IT TO WEEK " + Week.ToString();

        if (PlayersTotalGoatsEaten == 1)
        {
            _weekLoseText.text += " AND ATE " + PlayersTotalGoatsEaten.ToString() + " GOAT!";
        }
        else
        {
            _weekLoseText.text += " AND ATE " + PlayersTotalGoatsEaten.ToString() + " GOATS!";
        }

        _weekLoseText.text += "\nTHEN YOU RAN OUT OF bLoOd...";

        _restartButton.gameObject.SetActive(true);

        EventSystem eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(_restartButton.gameObject);

        //ScreenCapture.CaptureScreenshot("D:/Repos/ChupacabraGame/Screenshots/GameOverMenu.png");
    }
}
