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
        if (RobotData.Instance.IsTestRun) enabled = false;

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
            GameManager.Instance.CodeErrorTrigger(error);
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
        // TODO idea: maybe let user run code in test run so that they can see the levelInfo variables in the console
        // maybe need to disable function in RobotFunction, use `if (istestrun) return`
        if (RobotData.Instance.IsTestRun) return;

        levelInfo = DataBuffer.Instance.Get<LevelInfo>();
        startCode = true;
        RobotData.Instance.Script = CodeManager.Instance.GenerateNewScript();
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
        // in case some players try to use default Vector2 functions like Set()
        // pass in a copy instead of the original Vector2
        script.Globals["currentPos"] = CopyVector2((Vector2)gameObject.transform.position);
        script.Globals["winningPos"] = CopyVector2(levelInfo.winningPos);
        script.Globals["enemiesPos"] = CopyVector2Array(levelInfo.enemiesPos);
        script.Globals["platformPos"] = levelInfo.floatingPlatformPos;
        script.Globals["trapPos"] = levelInfo.trapsPos;
    }

    private Vector2 CopyVector2(Vector2 toCopy)
    {
        return new Vector2(toCopy.x, toCopy.y);
    }

    private Vector2[] CopyVector2Array(Vector2[] toCopy)
    {
        Vector2[] result = new Vector2[toCopy.Length];

        for (int i = 0; i < toCopy.Length; i++)
        {
            result[i] = CopyVector2(toCopy[i]);
        }

        return result;
    }
}
