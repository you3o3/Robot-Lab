using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CodingSceneUI : SingletonWMonoBehaviour<CodingSceneUI>
{
    public static CodingSceneUI Instance { get; private set; }

    private void Awake()
    {
        Instance = CreateInstance(Instance, false);
    }

    [Header("Coding panel")]
    [SerializeField] private TextMeshProUGUI levelInformationTMP;
    [SerializeField] private TMP_InputField codeInput;
    private LevelLoader levelLoader;

    [Header("Popup windows")]
    [SerializeField] private GameObject errorPopupWindow;
    [SerializeField] private TextMeshProUGUI errorText;

    [SerializeField] private GameObject successPopupWindow;

    [Header("Console")]
    [SerializeField] private TextMeshProUGUI consoleTextMeshPro;
    [SerializeField] private GameObject plusImage;
    [SerializeField] private GameObject minusImage;

    private bool enableConsole = false;

    private Dialogue[] dialogues;
    private bool dialogueTriggerOnce = true;

    private void Start()
    {
        levelLoader = GameObject.Find("Scene Loader").GetComponent<LevelLoader>();

        // get level and respective time limit
        int level = DataBuffer.Instance.level;
        if (level > GeneralInfo.TotalLevels)
        {
            Debug.Log("Level exceed");
            Dialogue comingSoonDialogue = DialogueTrigger.Instance.GetDialogueByString("Coming Soon");
            comingSoonDialogue.eventAfterDialogue.AddListener(OnBackButtonPress);
            this.dialogues = new Dialogue[] { comingSoonDialogue };
            dialogueTriggerOnce = false;
            return;
        }

        levelInformationTMP.text = string.Format("Level: {0}   Time limit: {1}   Robot: none",
            level, Utility.TimeToString(GeneralInfo.TimeLimit[level]));

        // the robot select info is carried to next game TODO maybe save which robot the code use in user computer too?
        SelectRobot(RobotData.Instance.Robot);
        string codeSave = RobotInfo.codes.TryGetValue(DataBuffer.Instance.level, out codeSave) ? codeSave : "";
        codeInput.text = codeSave;

        // change background music
        BGMusicManager.Instance.SwitchPart(0);

        // if current level has dialogue then present them all TODO maybe not present them all, allow present at some point
        Dialogue[] dialogues = DialogueTrigger.Instance.GetDialoguesOfLevel(level);
        this.dialogues = dialogues;
    }

    private void Update()
    {
        if (dialogues != null)
        {
            DialogueTrigger.Instance.TriggerScheduledDialogues(dialogues, dialogueTriggerOnce);

            dialogues = null;
            dialogueTriggerOnce = true;
        }
    }

    public void AccounceError(string error)
    {
        errorText.text = error;
        errorPopupWindow.SetActive(true);
    }

    public void AccounceSuccess()
    {
        successPopupWindow.SetActive(true);
    }

    public void SelectRobot(int robot)
    {
        if (robot < 0 || robot >= RobotInfo.RobotNames.Length)
        {
            Debug.LogError("robot index out of range");
            return;
        }
        if (robot != 0)
        {
            string original = levelInformationTMP.text;
            int robotTextIdx = original.LastIndexOf(':');
            if (robotTextIdx == -1) Debug.LogError("cannot find ':' in levelInformationTextMeshPro");
            levelInformationTMP.text = original.Substring(0, robotTextIdx + 1) + " " + RobotInfo.RobotNames[robot];
        }
        RobotData.Instance.Robot = robot;
    }

    private void ToGameScene(bool isTestRun)
    {
        RobotData.Instance.IsTestRun = isTestRun;
        RobotData.Instance.EnableConsole = enableConsole;

        levelLoader.LoadScene(GeneralInfo.sceneIdx["GameScene"]);
    }

    public void OnTestRunButtonPress()
    {
        CodeManager.Instance.SaveCode(codeInput.text);
        if (!CodeManager.Instance.CheckRobotInfoError()) return;
        ToGameScene(true);
    }

    public void OnRunButtonPress()
    {
        CodeManager.Instance.SaveCode(codeInput.text);
        if (!CodeManager.Instance.Compile(codeInput.text, false)) return;
        ToGameScene(false);
    }

    public void OnBackButtonPress()
    {
        CodeManager.Instance.SaveCode(codeInput.text);
        DataBuffer.Instance.level = 0;
        levelLoader.LoadScene(GeneralInfo.sceneIdx["LevelSelectScene"]);
    }

    public void OnCompileButtonPress()
    {
        CodeManager.Instance.SaveCode(codeInput.text);
        CodeManager.Instance.Compile(codeInput.text);
    }

    public void OnConsoleButtonPress()
    {
        enableConsole = !enableConsole;
        consoleTextMeshPro.text = enableConsole ? "Disable console" : "Enable console";
        if (enableConsole)
        {
            plusImage.SetActive(false);
            minusImage.SetActive(true);
        }
        else
        {
            plusImage.SetActive(true);
            minusImage.SetActive(false);
        }
    }

    public void OnHintButtonPress()
    {
        Dialogue hint = DialogueTrigger.Instance.GetHintOfLevel(DataBuffer.Instance.level);
        if (hint == null) return;
        DialogueTrigger.Instance.TriggerDialogue(hint, false);
    }
}
