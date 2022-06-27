using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;

public class CodeRunner : MonoBehaviour
{
    private bool startCode = false;
    private Script script;
    private LevelInfo levelInfo;

    void Start()
    {
        RobotData.Instance.Functions.Set(gameObject);
        script = RobotData.Instance.Script;
    }

    void Update()
    {
        if (!startCode) return;
        if (!GameManager.Instance.PlayerGaming) return;

        UpdateVariables();
        (DynValue result, string error) = Run(RobotData.Instance.Script, RobotData.Instance.Code);
        if (error != null)
        {
            if (RobotData.Instance.IsTestRun)
            {
                UIManager.Instance.AddCacheConsoleText(error);
            }
            else
            {
                GameManager.Instance.CodeErrorTrigger(error);
            }
        }

        UIManager.Instance.PrintCacheConsoleText();

        // lua table is 1-indexed
        //for (int i = 1; i <= result.Table.Length; i++)
        //{
        //    Debug.Log("i + " + i + " " + result.Table.Get(i).Type);
        //}
    }

    public void StartAutoRun()
    {
        levelInfo = DataBuffer.Instance.levelInfo;
        startCode = true;
        RobotData.Instance.Script = ScriptManager.GenerateNewScript();
    }

    public static (DynValue Result, string Error) Run(Script script, string code)
    {
        try
        {
            DynValue val = script.DoString(code);
            return (val, null);
        }
        catch (InterpreterException e)
        {
            Debug.Log(string.Format("Error when running code: {0}", e.DecoratedMessage));
            return (null, e.DecoratedMessage);
        }
    }

    private void UpdateVariables()
    {
        levelInfo.UpdateInfo();
        ScriptManager.UpdateGlobalVariables(script, gameObject, levelInfo);
    }
}
