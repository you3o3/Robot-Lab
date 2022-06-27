using System;
using UnityEngine;

public class CodeManager : SingletonWMonoBehaviour<CodeManager>
{
    public static CodeManager Instance { get; private set; }

    private void Awake()
    {
        Instance = CreateInstance(Instance, false);
    }

    public bool CheckRobotInfoError()
    {
        if (RobotData.Instance.Robot == 0)
        {
            CodingSceneUI.Instance.AccounceError("You did not choose your robot!");
            return false;
        }
        return true;
    }

    public bool Compile(string code, bool showSuccessWindow = true)
    {
        SaveCode(code);
        if (!CheckRobotInfoError()) return false;

        RobotData.Instance.Script = ScriptManager.GenerateNewScript();
        (_, string error)  = CodeRunner.Run(RobotData.Instance.Script, RobotData.Instance.Code);
        if (error != null)
        {
            CodingSceneUI.Instance.AccounceError(error);
            return false;
        }
        if (showSuccessWindow)
            CodingSceneUI.Instance.AccounceSuccess();

        return true;
    }

    public void SaveCode(string code)
    {
        RobotInfo.codes[DataBuffer.Instance.level] = code;
        RobotData.Instance.Code = code;
    }
}
