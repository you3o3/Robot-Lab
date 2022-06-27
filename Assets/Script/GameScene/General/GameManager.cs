using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonWMonoBehaviour<GameManager>
{
    [Header("Initialize Game")]
    [SerializeField] private CinemachineVirtualCamera playerFollowCamera;
    [SerializeField] private GameObject[] playerBullets;

    [Header("Game Setting")]
    [SerializeField] private float restartDelayDuration = 1.5f;

    [Header("All Levels")]
    [SerializeField] private GameObject allLevels;

    private int currLevel;

    public GameObject Player { private set; get; }

    private LevelLoader levelLoader;
    private float totalGameTime, timeLimit;
    private bool gamePaused, stopTimer; // only determine whether the game is paused, should not be used when game ends

    public bool PlayerGaming { get; set; } = false; // determine whether the game paused or ended

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = CreateInstance(Instance, false);
    }

    private void Start()
    {
        levelLoader = GameObject.Find("Scene Loader").GetComponent<LevelLoader>();

        // activate level
        currLevel = DataBuffer.Instance.level;
        GameObject level = allLevels.transform.GetChild(currLevel - 1).gameObject;
        level.SetActive(true);

        Debug.Log(level.name);

        DataBuffer.Instance.levelInfo = new LevelInfo(level);

        // instantiate player FIXME always initialize at vector3.zero
        Player = Instantiate(RobotInfo.RobotPrefabs[RobotData.Instance.Robot],
            Vector3.zero, Quaternion.identity);
        playerFollowCamera.Follow = Player.transform;
        Player.GetComponent<PlayerAttack>().Projectiles = playerBullets;

        Player.GetComponent<CodeRunner>().StartAutoRun();

        timeLimit = GeneralInfo.TimeLimit[currLevel];
        PlayerGaming = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (stopTimer)
        {
            return;
        }
        //FIXME check timer meet time limit
        if (!RobotData.Instance.IsTestRun && totalGameTime >= timeLimit)
        {
            PlayerTimesUp();
        }
        totalGameTime += Time.deltaTime;
        UIManager.Instance.UpdateTimeUI(totalGameTime);
    }

    public bool IsGamePaused()
    {
        return gamePaused;
    }

    public void PlayerTimesUp()
    {
        stopTimer = true;
        PlayerGaming = false;
        UIManager.Instance.ShowTimeoutPopOutScreen();
    }

    public void PlayerDied()
    {
        stopTimer = true;
        PlayerGaming = false;
        if (RobotData.Instance.IsTestRun)
            Invoke("RestartGame", restartDelayDuration);
        else
            UIManager.Instance.ShowDiedPopOutScreen();
    }

    public void PlayerWon()
    {
        stopTimer = true;
        PlayerGaming = false;
        if (RobotData.Instance.IsTestRun)
        {
            UIManager.Instance.ShowTestRunWonPopOutScreen();
        }
        else
        {
            GeneralInfo.LevelReached = Mathf.Max(GeneralInfo.LevelReached, currLevel + 1);
            UIManager.Instance.ShowWonPopOutScreen();
        }
    }

    public void CodeErrorTrigger(string error)
    {
        stopTimer = true;
        PlayerGaming = false;
        if (RobotData.Instance.IsTestRun)
        {
            Debug.LogError("an error in running code but it is test run (no code should be running)");
        }
        UIManager.Instance.ShowErrorPopOutScreen(error);
    }

    public void ToNextLevel()
    {
        DataBuffer.Instance.level = currLevel + 1;
        Exit();
    }

    public void Exit()
    {
        LeaveGameToScene(GeneralInfo.sceneIdx["CodingScene"]);
    }

    public void PauseGame()
    {
        if (gamePaused) return;
        PlayerGaming = false;
        gamePaused = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        if (!gamePaused) return;
        PlayerGaming = true;
        gamePaused = false;
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        LeaveGameToScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LeaveGameToScene(int buildIndex)
    {
        PlayerGaming = false;
        RobotData.Instance.Functions.Reset();
        levelLoader.LoadScene(buildIndex);
        ResumeGame();
    }
}
