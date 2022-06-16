using System;
using MoonSharp.Interpreter;
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

    public Script GenerateNewScript()
    {
        Script script = new Script(CoreModules.Preset_HardSandbox);

        RobotFunction functions = RobotData.Instance.Functions;

        script.Globals["walk"] = (Action<float>)functions.Walk;
        script.Globals["jump"] = (Action)functions.Jump;
        script.Globals["fire"] = (Action)functions.Fire;

        // redirect print() function in lua to unity self made console
        script.Options.DebugPrint = s =>
        {
            if (UIManager.Instance != null)
                UIManager.Instance.AddCacheConsoleText(s);
        };

        // types in unity that are allowed in lua
        UserData.RegisterType<Vector2>();
        UserData.RegisterType<LevelInfo.HorizontalBlock>();

        // constants
        int robot = RobotData.Instance.Robot;

        script.Globals["maxJumpDistance"] = RobotInfo.MaxJumpDistance[robot];

        // provided variables that might be changed in game
        script.Globals["currentPos"] = Vector2.one;
        script.Globals["winningPos"] = Vector2.zero;
        script.Globals["enemiesPos"] = new Vector2[0];
        script.Globals["platformPos"] = new LevelInfo.HorizontalBlock[0];
        script.Globals["trapPos"] = new LevelInfo.HorizontalBlock[0];

        return script;
    }

    public bool Compile(string code, bool showSuccessWindow = true)
    {
        SaveCode(code);
        if (!CheckRobotInfoError()) return false;

        RobotData.Instance.Script = GenerateNewScript();
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
        RobotInfo.codes[DataBuffer.Instance.Get<int>("level")] = code;
        RobotData.Instance.Code = code;
    }

}
