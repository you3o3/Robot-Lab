using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : Attack
{
    private PlayerMovement playerMovement;
    private PlayerInteraction playerInteraction;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerInteraction = GetComponent<PlayerInteraction>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!GameManager.Instance.PlayerGaming) return;
        if (!enableAttack) return;
        
        direction = (transform.localScale.x > 0) ? 1 : -1;
        cooldownTimer += Time.deltaTime;

        ////////////////////////////////////////////////////////////////
        /// Manual operation
        ////////////////////////////////////////////////////////////////

        if (!RobotData.Instance.IsTestRun) return;
        if (playerInteraction.PlayerDisabled) return;

        if (Input.GetKey(KeyCode.J))
        {
            Attack();
        }
    }

    public void Attack()
    {
        if (!enableAttack) return;
        if (cooldownTimer > attackCooldown && playerMovement.CanAttack())
        {
            Fire(true);
        }
    }

}
