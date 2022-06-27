using System;
using MoonSharp.Interpreter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboFunc : RobotFunction
{

}

public class JumpoFunc : RobotFunction
{

}

public class FireoFunc : RobotFunction
{

}

public class ShortoFunc : RobotFunction
{

}

public class CheatoFunc : RobotFunction
{
    public override void AddUserFunctions(Script script)
    {
        base.AddUserFunctions(script);

        //TODO add functions
    }
}
