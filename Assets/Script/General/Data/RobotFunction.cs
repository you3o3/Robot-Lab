using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    //TODO let player hardcode? eg walk right for 1s

    public virtual void Walk(float direction)
    {
        if (robot == null) return;
        robot.movement.Move(direction);
    }

    public virtual void Jump()
    {
        if (robot == null) return;
        robot.movement.Jump();
    }

    public virtual void Fire()
    {
        if (robot == null) return;
        robot.attack.Attack();
    }
}