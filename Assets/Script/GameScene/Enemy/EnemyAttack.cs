using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : Attack
{
    [Header("Box Collider")]
    [SerializeField] private BoxCollider2D boxCollider;
    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    [Header("Sight Setting")]
    [SerializeField] private float sightDistance;
    [SerializeField] private float sightRange;
    
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (!GameManager.Instance.PlayerGaming) return;
        if (!enableAttack) return;
        direction = (transform.localScale.x > 0) ? 1 : -1;
        if (PlayerInSight() && cooldownTimer > attackCooldown)
        {
            Fire();
        }
        cooldownTimer += Time.deltaTime;
    }

    private bool PlayerInSight()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * sightRange * transform.localScale.x * sightDistance
            , new Vector3(boxCollider.bounds.size.x * sightRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z)
            , 0, Vector2.left, 0, playerLayer);
        return raycastHit.collider != null;
    }

    private void OnDrawGizmos()
    {
        // draw the sight box
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * sightRange * transform.localScale.x * sightDistance
            , new Vector3(boxCollider.bounds.size.x * sightRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}
