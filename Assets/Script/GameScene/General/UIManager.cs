using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : SingletonWMonoBehaviour<UIManager>
{
    [Header("Time")]
    [SerializeField] private TextMeshProUGUI timeText;

    [Header("Win/Lose popout screen")]
    [SerializeField] private GameObject diedPopOutScreen;
    [SerializeField] private GameObject timeoutPopOutScreen;
    [SerializeField] private GameObject wonPopOutScreen;
    [SerializeField] private GameObject testRunWonPopOutScreen;

    [Header("Error popout screen")]
    [SerializeField] private GameObject errorPopOutScreen;
    [SerializeField] private TextMeshProUGUI errorText;

    [Header("Console")]
    [SerializeField] private GameObject consoleScreen;
    [SerializeField] private TMP_InputField consoleText;

    private List<string> cacheConsoleText = new();

    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        Instance = CreateInstance(Instance, false);
    }

    private void Start()
    {
        if (RobotData.Instance.EnableConsole)
            consoleScreen.SetActive(true);

        // change background music
        BGMusicManager.Instance.SwitchPart(2, 0.7f);
    }

    public void UpdateTimeUI(float time)
    {
        timeText.text = Utility.TimeToString(time);
    }

    public void AddCacheConsoleText(string text)
    {
        if (!RobotData.Instance.EnableConsole) return;
        cacheConsoleText.Add(text);
    }

    public void PrintCacheConsoleText()
    {
        consoleText.text = "";
        foreach (string s in cacheConsoleText)
        {
            if (s == null) continue;
            consoleText.text += s + "\n";
        }
        cacheConsoleText.Clear();
    }

    public void ShowDiedPopOutScreen()
    {
        diedPopOutScreen.SetActive(true);
    }

    public void ShowTimeoutPopOutScreen()
    {
        timeoutPopOutScreen.SetActive(true);
    }

    public void ShowWonPopOutScreen()
    {
        wonPopOutScreen.SetActive(true);
    }

    public void ShowErrorPopOutScreen(string error = "")
    {
        errorPopOutScreen.SetActive(true);
        errorText.text = error;
    }

    public void ShowTestRunWonPopOutScreen()
    {
        testRunWonPopOutScreen.SetActive(true);
    }

    public void OnPauseButtonPress()
    {
        GameManager.Instance.PauseGame();
        Debug.Log("Pause button pressed");
    }

    public void OnResumeButtonPress()
    {
        GameManager.Instance.ResumeGame();
        Debug.Log("Resume button pressed");
    }

    public void OnRestartButtonPress()
    {
        GameManager.Instance.RestartGame();
        Debug.Log("Restart button pressed");
    }

    public void OnNextLevelButtonPress()
    {
        GameManager.Instance.ToNextLevel();
        Debug.Log("Next Level button pressed");
    }

    public void OnExitButtonPress()
    {
        GameManager.Instance.Exit();
        Debug.Log("Exit button pressed");
    }
}
