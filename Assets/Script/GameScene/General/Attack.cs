using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    [Header("Attack Setting")]
    [SerializeField] protected bool enableAttack = true;
    [SerializeField] protected float attackCooldown;
    [Header("Projectile Setting")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] projectiles;
    [SerializeField] private float projectileSpeed = 10;
    protected float cooldownTimer = Mathf.Infinity;
    protected int direction;

    public GameObject[] Projectiles
    {
        set { projectiles = value; }
    }


    // Update is called once per frame
    protected virtual void Update()
    {
        if (!enableAttack) return;
        direction = (transform.localScale.x > 0) ? 1 : -1;
        //if (cooldownTimer > attackCooldown)
        //{
        //    Fire();
        //}
        cooldownTimer += Time.deltaTime;
    }

    protected void Fire(bool isPlayer = false)
    {
        cooldownTimer = 0;
        GameObject projectile = FindAvailableProjectile();
        projectile.transform.position = firePoint.position;
        projectile.GetComponent<Projectile>().FireThisProjectile(direction, projectileSpeed, isPlayer);
    }

    private GameObject FindAvailableProjectile()
    {
        foreach (GameObject projectile in projectiles)
        {
            if (!projectile.activeInHierarchy) return projectile;
        }
        return projectiles[0];
    }
}
