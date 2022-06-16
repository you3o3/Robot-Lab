using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralInfo : ScriptableObject
{
    public static readonly Dictionary<string, int> sceneIdx = new()
    {
        { "MenuScene", 0 },
        { "LevelSelectScene", 1 },
        { "CodingScene", 2 },
        { "GameScene", 3 }
    };

    public static int TotalLevels { get; } = 5;

    public static int LevelReached { get; set; } = 1;

    public static string[] HumanNames { get; } =
        { "Professor X", "John" };

    public static string[] AllNames
    {
        get
        {
            string[] allNames = new string[RobotInfo.RobotNames.Length + HumanNames.Length];
            RobotInfo.RobotNames.CopyTo(allNames, 0);
            HumanNames.CopyTo(allNames, RobotInfo.RobotNames.Length);
            return allNames;
        }
    }

    public static float[] TimeLimit { get; } =
    {
        0,      // level 0: null

        5,      // level 1: walk
        5,      // level 2: jump
        5,      // level 3: fire
        5,      // level 4: if
        5       // level 5: more if
    };

    /**

    Solutions:

    -----------------------  Lv1  -----------------------

    walk(1)


    -----------------------  Lv2  -----------------------

    walk(1)
    jump()


    -----------------------  Lv3  -----------------------

    walk(1)
    fire()

    or

    walk(1)
    jump()

    
    -----------------------  Lv4  -----------------------
    (offset 1.7)
    
    for i = 1, #trapPos do
        local tPos = trapPos[i]
        local distx = currentPos.x + maxJumpDistance.x
        if (currentPos.x <= tPos.xStart and distx >= tPos.xEnd + 1.7) then
            jump()
        end
    end
    walk(1)


    -----------------------  Lv5  -----------------------
    
    if (#enemiesPos != 0) then
	    walk(-1) fire()
    else
	    isTrap = false
        for i = 1, #trapPos do
            local tPos = trapPos[i]
            local distx = currentPos.x + maxJumpDistance.x
            if (currentPos.x <= tPos.xStart or distx <= tPos.xEnd) then
                isTrap = true
            end
        end
        if not isTrap then
            jump()
        end
        walk(1)
    end
     
     */

    // You can command the robot to jump by typing `jump()`.
    // In some levels, you might want to use particular robot to pass.
    // Try to choose jumpo and test the level!

    // Choose jumpo, and then type `walk(1)` and `jump()`


    // You can use `fire()` to instruct robot to fire a bullet.
    // And, in some levels like this one, you can find multiple ways to complete the level!

    // Try to use `fire()` or `jump()`!


    // The for loop and if statement are extremely useful in many cases. Take a look on the syntax by searching "Lua programming language" in the Internet!
    // You would want to use `currentPos`, `maxJumpDistance` and `trapPos` in this level.
    // The position in `currentPos`, `winningPos` and `enemiesPos` is the center of the robot or enemy, while the position of HorizontalBlock like `trapPos` and `platformPos` is the bottom left of the block!
    // Use console to help you if you want a better understanding!

    // You can use for loop to loop through the trapPos. Use if statement inside the loop to test whether a trap is in front of the robot. If it is, instruct the robot to jump.
    // You would want to add some offset to compensate the height of the traps' hitting box.


    // You would want to kill the enemy first, and then go to the goal.

}
