using System.Collections;
using System.Collections.Generic;
using MoonSharp.Interpreter;
using UnityEngine;

public class RobotData
{
    public static RobotData Instance { get; } = new();
    private RobotData() { }

    // Content
    private int robot;
    public string RobotName { get; private set; }
    
    public bool IsTestRun { get; set; }

    public string Code { get; set; }
    public Script Script { get; set; }
    public RobotFunction Functions { get { return RobotInfo.GetRobotFunctionFromIdx(Instance.robot); } }

    public bool EnableConsole { get; set; } = false;

    public int Robot
    {
        get { return robot; }
        set { robot = value; RobotName = RobotInfo.RobotNames[value]; }
    }
}
