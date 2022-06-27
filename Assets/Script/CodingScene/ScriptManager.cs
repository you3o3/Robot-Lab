using UnityEngine;
using MoonSharp.Interpreter;

public static class ScriptManager
{
    public static Script GenerateNewScript()
    {
        Script script = new Script(CoreModules.Preset_HardSandbox);

        RobotFunction functions = RobotData.Instance.Functions;

        functions.AddUserFunctions(script);

        // redirect print() function in lua to unity self made console
        script.Options.DebugPrint = s =>
        {
            if (UIManager.Instance != null)
                UIManager.Instance.AddCacheConsoleText(s);
        };

        // types in unity that are allowed in lua
        UserData.RegisterType<Vector2>();
        UserData.RegisterType<LevelInfo.RectBlock>();

        // constants
        int robot = RobotData.Instance.Robot;

        InitGlobalVariables(script, robot);

        return script;
    }

    private static void InitGlobalVariables(Script script, int robot)
    {
        // constants
        script.Globals["maxJumpDistance"] = RobotInfo.MaxJumpDistance[robot];

        // provided variables that might be changed in game
        script.Globals["currentPos"] = Vector2.one;
        script.Globals["winningPos"] = Vector2.zero;
        script.Globals["enemiesPos"] = new Vector2[0];
        script.Globals["platformPos"] = new LevelInfo.RectBlock[0];
        script.Globals["trapPos"] = new LevelInfo.RectBlock[0];
    }

    public static void UpdateGlobalVariables(Script script, GameObject player, LevelInfo levelInfo)
    {
        // in case some players try to use default Vector2 functions like Set()
        // pass in a copy instead of the original Vector2
        script.Globals["currentPos"] = CopyVector2((Vector2)player.transform.position);
        script.Globals["winningPos"] = CopyVector2(levelInfo.winningPos);
        script.Globals["enemiesPos"] = CopyVector2Array(levelInfo.enemiesPos);
        script.Globals["platformPos"] = levelInfo.floatingPlatformPos;
        script.Globals["trapPos"] = levelInfo.trapsPos;
    }

    private static Vector2 CopyVector2(Vector2 toCopy)
    {
        return new Vector2(toCopy.x, toCopy.y);
    }

    private static Vector2[] CopyVector2Array(Vector2[] toCopy)
    {
        Vector2[] result = new Vector2[toCopy.Length];

        for (int i = 0; i < toCopy.Length; i++)
        {
            result[i] = CopyVector2(toCopy[i]);
        }

        return result;
    }
}
