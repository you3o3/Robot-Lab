using UnityEngine;
using MoonSharp.Interpreter;
using System.Collections;
using System.Collections.Generic;
using System;

public abstract class RobotFunction
{
    protected class Robot
    {
        public GameObject robotObj;

        public PlayerMovement movement;
        public PlayerAttack attack;
        public PlayerInteraction interaction;

        public Robot(GameObject robot)
        {
            robotObj = robot;

            movement = robot.GetComponent<PlayerMovement>();
            attack = robot.GetComponent<PlayerAttack>();
            interaction = robot.GetComponent<PlayerInteraction>();
        }
    }

    protected Robot robot;

    public void Set(GameObject robot)
    {
        this.robot = new Robot(robot);
    }

    public void Reset()
    {
        robot = null;
    }

    public virtual void AddUserFunctions(Script script)
    {
        script.Globals["walk"] = (Action<float>)Walk;
        script.Globals["jump"] = (Action)Jump;
        script.Globals["fire"] = (Action)Fire;
    }

    //TODO let player hardcode? eg walk right for 1s

    public virtual void Walk(float direction)
    {
        if (robot == null) return;
        if (RobotData.Instance.IsTestRun) return;
        robot.movement.Move(direction);
    }

    public virtual void Jump()
    {
        if (robot == null) return;
        if (RobotData.Instance.IsTestRun) return;
        robot.movement.Jump();
    }

    public virtual void Fire()
    {
        if (robot == null) return;
        if (RobotData.Instance.IsTestRun) return;
        robot.attack.Attack();
    }
}
